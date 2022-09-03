using FooCommerce.Domain.Enums;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Worker.Contracts;
using FooCommerce.NotificationAPI.Worker.Events;

using MassTransit;

namespace FooCommerce.NotificationAPI.Worker.Consumers;

public class QueueNotificationPushInAppConsumer
    : IConsumer<QueueNotificationPushInApp>
{
    public async Task Consume(ConsumeContext<QueueNotificationPushInApp> context)
    {
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
            await context.RespondAsync<NotificationSendFaulted>(new
            {
                NotificationId = context.Message.NotificationId,
                Gateway = CommunicationType.Push_Notification
            });
        }
    }
}