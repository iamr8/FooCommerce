using System.Text;

using FooCommerce.Application.Helpers;
using FooCommerce.Application.Membership.Entities;
using FooCommerce.Application.Notifications.Services;
using FooCommerce.Domain.Interfaces;
using FooCommerce.Infrastructure.Helpers;
using FooCommerce.NotificationAPI.Consumers.Extensions;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Dtos;
using FooCommerce.NotificationAPI.Enums;
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
            var renderedTemplate = await context.Message.Factory.CreateEmailModelAsync(
                (NotificationTemplateEmailModel)context.Message.Template,
                options =>
                {
                    options.LocalDateTime = DateTime.UtcNow.ToLocal(context.Message.RequestInfo);
                    options.WebsiteUrl = context.Message.WebsiteUrl;
                });
            var receiver = context.Message.Options.Receiver.UserCommunications.Single(x => x.Type == context.Message.Template.Communication);

            QueueNotificationHandlerGuard.Check(renderedTemplate, context.Message, _logger);

            var mailCredential = _clientService.GetAvailableMailboxCredentials();
            var mime = new MimeMessage
            {
                Subject = _localizer[context.Message.Options.Action.GetLocalizerKey()],
                Sender = new MailboxAddress(mailCredential.SenderAlias, mailCredential.SenderAddress),
                Importance = context.Message.IsImportant ? MessageImportance.High : MessageImportance.Normal,
                Body = new BodyBuilder { HtmlBody = renderedTemplate.Html.GetString() }.ToMessageBody(),
            };

            mime.From.Add(mime.Sender);
            mime.To.Add(new MailboxAddress(context.Message.Options.Receiver.Name, receiver.Value));

            var emailSent = true;
            if (!_environment.IsStaging())
            {
                await using var emailClient = await EmailClient.GetInstanceAsync(mailCredential.SenderAddress, mailCredential.Password, mailCredential.Server, mailCredential.SenderAlias, mailCredential.SmtpPort, context.CancellationToken);
                emailSent = await emailClient.SendAsync(mime, context.CancellationToken);
                if (!emailSent)
                {
                    _logger.LogError("Unable to send {0} to User {1}", context.Message.Template.Communication, context.Message.Options.Receiver.UserId);
                }

                await emailClient.DisposeAsync();
            }

            if (emailSent)
            {
                await context.RespondAsync<NotificationSent>(new
                {
                    NotificationId = context.Message.NotificationId,
                    Gateway = context.Message.Template.Communication
                });

                var token = context.Message.Options.Bag.OfType<AuthToken>().FirstOrDefault();
                if (token != null)
                {
                    await context.Publish<UpdateAuthTokenState>(new
                    {
                        AuthTokenId = token.Id,
                        State = UserNotificationUpdateState.Sent
                    }, context.CancellationToken);
                }
            }
            else
            {
                await context.RespondAsync<NotificationFailed>(new
                {
                    NotificationId = context.Message.NotificationId,
                    Gateway = context.Message.Template.Communication
                });
            }
        }
    }
}