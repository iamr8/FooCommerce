using FooCommerce.Application.Communications.Enums;
using FooCommerce.NotificationAPI.Worker.Contracts;
using FooCommerce.NotificationAPI.Worker.Events;

using MassTransit;

namespace FooCommerce.NotificationAPI.Worker.Consumers;

public class QueueNotificationPushInAppConsumer
    : IConsumer<QueueNotificationPushInApp>
{
    private readonly ILogger<QueueNotificationPushInAppConsumer> _logger;

    public QueueNotificationPushInAppConsumer(ILogger<QueueNotificationPushInAppConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<QueueNotificationPushInApp> context)
    {
        var receiver = context.Message.Receiver.UserCommunications.Single(x => x.Type == CommunicationType.Push_Notification);

        // save user context.Message in database
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