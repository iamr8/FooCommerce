﻿using FooCommerce.Common.Helpers;
using FooCommerce.Common.Localization;
using FooCommerce.Domain.Enums;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Worker.Contracts;
using FooCommerce.NotificationAPI.Worker.Events;
using FooCommerce.NotificationAPI.Worker.Models;
using FooCommerce.NotificationAPI.Worker.Services;
using MassTransit;

using MimeKit;

namespace FooCommerce.NotificationAPI.Worker.Consumers;

public class QueueNotificationEmailConsumer
    : IConsumer<QueueNotificationEmail>
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
        mime.To.Add(new MailboxAddress(context.Message.Receiver.Name, context.Message.Receiver.Address));

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
                // Add Action for PostProcess

                //var token = context.Message.Bag.OfType<AuthToken>().FirstOrDefault();
                //if (token != null)
                //{
                //    await context.Publish<UpdateAuthTokenState>(new
                //    {
                //        AuthTokenId = token.Id,
                //        State = UserNotificationState.Sent
                //    }, context.CancellationToken);
                //}
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