using System.Text;

using FooCommerce.Application.Dtos.Notifications;
using FooCommerce.Application.Entities.Membership;
using FooCommerce.Application.Enums.Notifications;
using FooCommerce.Application.Helpers;
using FooCommerce.Application.Publishers.Membership;
using FooCommerce.Application.Services.Notifications;
using FooCommerce.Domain.Interfaces;
using FooCommerce.Infrastructure.Helpers;
using FooCommerce.NotificationAPI.Models;
using FooCommerce.NotificationAPI.Publishers;

using MassTransit;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MimeKit;

namespace FooCommerce.NotificationAPI.Consumers
{
    public class SendNotificationEmailConsumer : IConsumer<SendNotificationEmail>
    {
        private readonly ILogger<SendNotificationEmailConsumer> _logger;
        private readonly INotificationClientService _clientService;
        private readonly ILocalizer _localizer;
        private readonly IWebHostEnvironment _environment;

        public SendNotificationEmailConsumer(ILogger<SendNotificationEmailConsumer> logger,
            INotificationClientService clientService,
            ILocalizer localizer,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _clientService = clientService;
            _localizer = localizer;
            _environment = environment;
        }

        public async Task Consume(ConsumeContext<SendNotificationEmail> context)
        {
            var renderedTemplate = await context.Message.Options.Factory.CreateEmailModelAsync(
                (NotificationTemplateEmailModel)context.Message.Options.Template,
                options =>
                {
                    options.LocalDateTime = DateTime.UtcNow.ToLocal(context.Message.Options.RequestInfo);
                    options.WebsiteUrl = context.Message.Options.WebsiteUrl;
                });
            var receiver = context.Message.Options.Options.Receiver.UserCommunications.Single(x => x.Type == context.Message.Options.Template.Communication);

            SendNotificationHandlerGuard.Check(renderedTemplate, context.Message.Options, _logger);

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
                await using (var emailClient = await EmailClient.GetInstanceAsync(mailCredential.SenderAddress, mailCredential.Password, mailCredential.Server, mailCredential.SenderAlias, mailCredential.SmtpPort, context.CancellationToken))
                {
                    emailSent = await emailClient.SendAsync(mime, context.CancellationToken);
                    if (!emailSent)
                        _logger.LogError("Unable to send {0} to User {1}", context.Message.Options.Template.Communication, context.Message.Options.Options.Receiver.UserId);
                }
            }

            if (emailSent)
            {
                var token = context.Message.Options.Options.Bag.OfType<AuthToken>().FirstOrDefault();
                if (token != null)
                {
                    await context.Publish(new UpdateAuthTokenState(token.Id, UserNotificationUpdateState.Sent), context.CancellationToken);
                }
            }
        }
    }
}