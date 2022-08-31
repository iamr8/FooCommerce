using FooCommerce.Application.Communications.Enums;
using FooCommerce.NotificationAPI.Worker.Contracts;
using FooCommerce.NotificationAPI.Worker.Events;

using MassTransit;

namespace FooCommerce.NotificationAPI.Worker.Consumers;

public class QueueNotificationSmsConsumer
    : IConsumer<QueueNotificationSms>
{
    private readonly ILogger<QueueNotificationSmsConsumer> _logger;

    public QueueNotificationSmsConsumer(ILogger<QueueNotificationSmsConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<QueueNotificationSms> context)
    {
        var receiver = context.Message.Receiver.UserCommunications.Single(x => x.Type == CommunicationType.Mobile_Sms);

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