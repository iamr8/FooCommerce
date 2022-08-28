using FooCommerce.Application.Membership.Enums;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Events;

using MassTransit;

using Microsoft.Extensions.Logging;

namespace FooCommerce.NotificationAPI.Consumers;

public class QueueNotificationPushConsumer : IConsumer<QueueNotificationPush>
{
    private readonly ILogger<QueueNotificationPushConsumer> _logger;

    public QueueNotificationPushConsumer(ILogger<QueueNotificationPushConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<QueueNotificationPush> context)
    {
        var receiver = context.Message.Receiver.UserCommunications.Single(x => x.Type == CommunicationType.Push_Notification);

        // send Push Notification using relevant SDK

        var pushSent = true;
        if (pushSent)
        {
            await context.RespondAsync<NotificationSent>(new
            {
                NotificationId = context.Message.NotificationId,
                Gateway = CommunicationType.Push_Notification
            });
        }
        else
        {
            await context.RespondAsync<NotificationSendFailed>(new
            {
                NotificationId = context.Message.NotificationId,
                Gateway = CommunicationType.Push_Notification
            });
        }
    }
}