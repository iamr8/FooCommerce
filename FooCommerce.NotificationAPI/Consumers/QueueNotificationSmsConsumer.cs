using FooCommerce.Application.Membership.Entities;
using FooCommerce.Application.Membership.Enums;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Events;

using MassTransit;

using Microsoft.Extensions.Logging;

namespace FooCommerce.NotificationAPI.Consumers;

public class QueueNotificationSmsConsumer : IConsumer<QueueNotificationSms>
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
        else
        {
            await context.RespondAsync<NotificationFailed>(new
            {
                NotificationId = context.Message.NotificationId,
                Gateway = CommunicationType.Mobile_Sms
            });
        }
    }
}