using FooCommerce.Domain.Enums;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Worker.Contracts;
using FooCommerce.NotificationAPI.Worker.Events;

using MassTransit;

namespace FooCommerce.NotificationAPI.Worker.Consumers;

public class QueueNotificationSmsConsumer
    : IConsumer<QueueNotificationSms>
{
    public async Task Consume(ConsumeContext<QueueNotificationSms> context)
    {
        // TODO: SDK must be implemented
        var smsSent = true;
        if (smsSent)
        {
            await context.RespondAsync<NotificationSent>(new
            {
                NotificationId = context.Message.NotificationId,
                Gateway = CommunicationType.Mobile_Sms
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
                Gateway = CommunicationType.Mobile_Sms
            });
        }
    }
}