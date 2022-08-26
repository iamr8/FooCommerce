using FooCommerce.Application.Membership.Entities;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Dtos;
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
        var renderedTemplate = context.Message.Factory.CreateSmsModel(
            (NotificationTemplateSmsModel)context.Message.Template,
            options =>
            {
                options.WebsiteUrl = context.Message.WebsiteUrl;
            });
        var receiver = context.Message.Options.Receiver.UserCommunications.Single(x => x.Type == context.Message.Template.Communication);

        QueueNotificationHandlerGuard.Check(renderedTemplate, context.Message, _logger);

        // TODO: SDK must be implemented
        var smsSent = true;
        if (smsSent)
        {
            await context.RespondAsync<NotificationSent>(new
            {
                NotificationId = context.Message.NotificationId,
                Gateway = context.Message.Template.Communication
            });

            var token = context.Message.Options.Bag.OfType<AuthToken>().FirstOrDefault();
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
                Gateway = context.Message.Template.Communication
            });
        }
    }
}