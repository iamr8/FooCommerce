using System.Text;

using FooCommerce.Application.Helpers;
using FooCommerce.Application.Membership.Entities;
using FooCommerce.Application.Membership.Publishers;
using FooCommerce.Application.Notifications.Dtos;
using FooCommerce.Application.Notifications.Enums;
using FooCommerce.Application.Notifications.Services;
using FooCommerce.Domain.Interfaces;
using FooCommerce.Infrastructure.Helpers;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Events;
using FooCommerce.NotificationAPI.Models;

using MassTransit;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MimeKit;

namespace FooCommerce.NotificationAPI.Consumers
{
    public class QueueNotificationEmailConsumer : IConsumer<QueueNotificationEmail>
    {
        private readonly ILogger<QueueNotificationEmailConsumer> _logger;
        private readonly INotificationClientService _clientService;
        private readonly ILocalizer _localizer;
        private readonly IWebHostEnvironment _environment;

        public QueueNotificationEmailConsumer(ILogger<QueueNotificationEmailConsumer> logger,
            INotificationClientService clientService,
            ILocalizer localizer,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _clientService = clientService;
            _localizer = localizer;
            _environment = environment;
        }

        public async Task Consume(ConsumeContext<QueueNotificationEmail> context)
        {
            var renderedTemplate = await context.Message.Options.Factory.CreateEmailModelAsync(
                (NotificationTemplateEmailModel)context.Message.Options.Template,
                options =>
                {
                    options.LocalDateTime = DateTime.UtcNow.ToLocal(context.Message.Options.RequestInfo);
                    options.WebsiteUrl = context.Message.Options.WebsiteUrl;
                });
            var receiver = context.Message.Options.Options.Receiver.UserCommunications.Single(x => x.Type == context.Message.Options.Template.Communication);

            QueueNotificationHandlerGuard.Check(renderedTemplate, context.Message.Options, _logger);

            var mailCredential = _clientService.GetAvailableMailboxCredentials();
            var mime = new MimeMessage
            {
                Subject = _localizer[context.Message.Options.Options.Action.GetLocalizerKey()],
                Sender = new MailboxAddress(mailCredential.SenderAlias, mailCredential.SenderAddress),
                Importance = context.Message.Options.IsImportant ? MessageImportance.High : MessageImportance.Normal,
                Body = new BodyBuilder { HtmlBody = renderedTemplate.Html.GetString() }.ToMessageBody(),
            };

            mime.From.Add(mime.Sender);
            mime.To.Add(new MailboxAddress(context.Message.Options.Options.Receiver.Name, receiver.Value));

            var emailSent = true;
            if (!_environment.IsStaging())
            {
                await using var emailClient = await EmailClient.GetInstanceAsync(mailCredential.SenderAddress, mailCredential.Password, mailCredential.Server, mailCredential.SenderAlias, mailCredential.SmtpPort, context.CancellationToken);
                emailSent = await emailClient.SendAsync(mime, context.CancellationToken);
                if (!emailSent)
                {
                    _logger.LogError("Unable to send {0} to User {1}", context.Message.Options.Template.Communication, context.Message.Options.Options.Receiver.UserId);
                }

                await emailClient.DisposeAsync();
            }

            if (emailSent)
            {
                await context.RespondAsync<NotificationSent>(new
                {
                    NotificationId = context.Message.NotificationId,
                    Gateway = context.Message.Options.Template.Communication
                });

                var token = context.Message.Options.Options.Bag.OfType<AuthToken>().FirstOrDefault();
                if (token != null)
                {
                    await context.Publish(new UpdateAuthTokenState(token.Id, UserNotificationUpdateState.Sent), context.CancellationToken);
                }
            }
            else
            {
                await context.RespondAsync<NotificationFailed>(new
                {
                    NotificationId = context.Message.NotificationId,
                    Gateway = context.Message.Options.Template.Communication
                });
            }
        }
    }
}