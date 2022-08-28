using System.Text;

using FooCommerce.Application.Membership.Entities;
using FooCommerce.Application.Membership.Enums;
using FooCommerce.Application.Notifications.Services;
using FooCommerce.Domain.Interfaces;
using FooCommerce.Infrastructure.Helpers;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Events;
using FooCommerce.NotificationAPI.Models;

using MassTransit;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MimeKit;

namespace FooCommerce.NotificationAPI.Consumers;

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
        var receiver = context.Message.Receiver.UserCommunications.Single(x => x.Type == CommunicationType.Email_Message);

        var mailCredentials = _clientService.GetAvailableMailboxCredentials();
        if (mailCredentials == null || !mailCredentials.Any())
            throw new NullReferenceException("Unable to find any credentials for sender mailbox.");

        var mailCredential = mailCredentials.First();

        var mime = new MimeMessage
        {
            Subject = _localizer[context.Message.Action],
            Sender = new MailboxAddress(mailCredential.SenderAlias, mailCredential.SenderAddress),
            Importance = context.Message.IsImportant ? MessageImportance.High : MessageImportance.Normal,
            Body = new BodyBuilder { HtmlBody = context.Message.Model.Html.GetString() }.ToMessageBody(),
        };

        mime.From.Add(mime.Sender);
        mime.To.Add(new MailboxAddress(context.Message.Receiver.Name, receiver.Value));

        var emailSent = true;
        if (!_environment.IsStaging())
        {
            await using var emailClient = await EmailClient.GetInstanceAsync(mailCredential.SenderAddress, mailCredential.Password, mailCredential.Server, mailCredential.SenderAlias, mailCredential.SmtpPort, context.CancellationToken);
            emailSent = await emailClient.SendAsync(mime, context.CancellationToken);
            if (!emailSent)
            {
                _logger.LogError("Unable to send {0} to User {1}", CommunicationType.Email_Message, context.Message.Receiver.UserId);
            }

            await emailClient.DisposeAsync();
        }

        if (emailSent)
        {
            await context.RespondAsync<NotificationSent>(new
            {
                NotificationId = context.Message.NotificationId,
                Gateway = CommunicationType.Email_Message
            });

            if (context.Message.Bag?.Any() == true)
            {
                var token = context.Message.Bag.OfType<AuthToken>().FirstOrDefault();
                if (token != null)
                {
                    await context.Publish<UpdateAuthTokenState>(new
                    {
                        AuthTokenId = token.Id,
                        State = UserNotificationUpdateState.Sent
                    }, context.CancellationToken);
                }
            }
        }
        else
        {
            await context.RespondAsync<NotificationSendFailed>(new
            {
                NotificationId = context.Message.NotificationId,
                Gateway = CommunicationType.Email_Message
            });
        }
    }
}