using FooCommerce.NotificationAPI.Consumers.Extensions;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Dtos;
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
        var renderedTemplate = context.Message.Factory.CreatePushModel(
            (NotificationTemplatePushModel)context.Message.Template,
            options =>
            {
                options.WebsiteUrl = context.Message.WebsiteUrl;
            });
        var receiver = context.Message.Options.Receiver.UserCommunications.Single(x => x.Type == context.Message.Template.Communication);

        QueueNotificationHandlerGuard.Check(renderedTemplate, context.Message, _logger);
        // send Push Notification using relevant SDK

        var pushSent = true;
        if (pushSent)
        {
            await context.RespondAsync<NotificationSent>(new
            {
                NotificationId = context.Message.NotificationId,
                Gateway = context.Message.Template.Communication
            });
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