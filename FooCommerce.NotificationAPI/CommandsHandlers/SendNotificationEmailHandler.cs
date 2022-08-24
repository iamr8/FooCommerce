using System.Text;

using FooCommerce.Application.Commands.Membership;
using FooCommerce.Application.Dtos.Notifications;
using FooCommerce.Application.Entities.Membership;
using FooCommerce.Application.Enums.Notifications;
using FooCommerce.Application.Helpers;
using FooCommerce.Application.Services.Notifications;
using FooCommerce.Domain.Interfaces;
using FooCommerce.Infrastructure.Helpers;
using FooCommerce.NotificationAPI.Commands;
using FooCommerce.NotificationAPI.Models;

using MediatR;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MimeKit;

namespace FooCommerce.NotificationAPI.CommandsHandlers
{
    public class SendNotificationEmailHandler : INotificationHandler<SendNotificationEmail>
    {
        private readonly ILogger<SendNotificationEmailHandler> _logger;
        private readonly INotificationClientService _clientService;
        private readonly ILocalizer _localizer;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IMediator _mediator;

        public SendNotificationEmailHandler(ILogger<SendNotificationEmailHandler> logger,
            INotificationClientService clientService,
            ILocalizer localizer,
            IConfiguration configuration,
            IWebHostEnvironment environment,
            IMediator mediator)
        {
            _logger = logger;
            _clientService = clientService;
            _localizer = localizer;
            _configuration = configuration;
            _environment = environment;
            _mediator = mediator;
        }

        public async Task Handle(SendNotificationEmail notification, CancellationToken cancellationToken)
        {
            var renderedTemplate = await notification.Options.Factory.CreateEmailModelAsync(
                (NotificationTemplateEmailModel)notification.Options.Template,
                options =>
                {
                    options.LocalDateTime = DateTime.UtcNow.ToLocal(notification.Options.RequestInfo);
                    options.WebsiteUrl = notification.Options.WebsiteUrl;
                });
            var receiver = notification.Options.Options.Receiver.UserCommunications.Single(x => x.Type == notification.Options.Template.Communication);

            SendNotificationHandlerGuard.Check(renderedTemplate, notification.Options, _logger);

            var mailCredential = _clientService.GetAvailableMailboxCredentials();
            var mime = new MimeMessage
            {
                Subject = _localizer[notification.Options.Options.Action.GetLocalizerKey()],
                Sender = new MailboxAddress(mailCredential.SenderAlias, mailCredential.SenderAddress),
                Importance = notification.Options.IsImportant ? MessageImportance.High : MessageImportance.Normal,
                Body = new BodyBuilder { HtmlBody = renderedTemplate.Html.GetString() }.ToMessageBody(),
            };

            mime.From.Add(mime.Sender);
            mime.To.Add(new MailboxAddress(notification.Options.Options.Receiver.Name, receiver.Value));

            var emailSent = true;
            if (!_environment.IsStaging())
            {
                await using (var emailClient = await EmailClient.GetInstanceAsync(mailCredential.SenderAddress, mailCredential.Password, mailCredential.Server, mailCredential.SenderAlias, mailCredential.SmtpPort, cancellationToken))
                {
                    emailSent = await emailClient.SendAsync(mime, cancellationToken);
                    if (!emailSent)
                        _logger.LogError("Unable to send {0} to User {1}", notification.Options.Template.Communication, notification.Options.Options.Receiver.UserId);
                }
            }

            if (emailSent)
            {
                var token = notification.Options.Options.Bag.OfType<AuthToken>().FirstOrDefault();
                if (token != null)
                {
                    await _mediator.Publish(new UpdateAuthTokenState(token.Id, UserNotificationUpdateState.Sent), cancellationToken);
                }
            }
        }
    }
}