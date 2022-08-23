using System.Text;

using FooCommerce.Application.Commands.Membership;
using FooCommerce.Application.Commands.Notifications;
using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Dtos.Notifications;
using FooCommerce.Application.Entities.Membership;
using FooCommerce.Application.Enums.Notifications;
using FooCommerce.Application.Helpers;
using FooCommerce.Domain.Interfaces;
using FooCommerce.Infrastructure.Helpers;
using FooCommerce.NotificationAPI.Models;

using MediatR;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MimeKit;

namespace FooCommerce.NotificationAPI.Commands
{
    public class SendNotificationEmailHandler : INotificationHandler<SendNotificationEmail>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<SendNotificationEmailHandler> _logger;
        private readonly ILocalizer _localizer;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IMediator _mediator;

        public SendNotificationEmailHandler(ILogger<SendNotificationEmailHandler> logger, ILocalizer localizer, IConfiguration configuration, IWebHostEnvironment environment, IMediator mediator, IDbConnectionFactory dbConnectionFactory)
        {
            _logger = logger;
            _localizer = localizer;
            _configuration = configuration;
            _environment = environment;
            _mediator = mediator;
            _dbConnectionFactory = dbConnectionFactory;
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

            var (username, password, server, smtpPort, senderName, domain) = _configuration.GetSection("Emails:NoReply").Get<EmailClientCredential>();
            var senderAddress = $"{username}@{domain}";
            var mime = new MimeMessage
            {
                Subject = _localizer[notification.Options.Options.Action.GetLocalizerKey()],
                Sender = new MailboxAddress(senderName, senderAddress),
                Importance = notification.Options.IsImportant ? MessageImportance.High : MessageImportance.Normal,
                Body = new BodyBuilder { HtmlBody = renderedTemplate.Html.GetString() }.ToMessageBody(),
            };

            mime.From.Add(mime.Sender);
            mime.To.Add(new MailboxAddress(notification.Options.Options.Receiver.Name, receiver.Value));

            var emailSent = true;
            if (!_environment.IsStaging())
            {
                await using (var emailClient = await EmailClient.GetInstanceAsync(senderAddress, password, server, senderName, smtpPort, cancellationToken))
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