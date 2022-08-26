using FooCommerce.Application.Notifications.Dtos;
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
        var renderedTemplate = context.Message.Options.Factory.CreatePushModel(
            (NotificationTemplatePushModel)context.Message.Options.Template,
            options =>
            {
                options.WebsiteUrl = context.Message.Options.WebsiteUrl;
            });
        var receiver = context.Message.Options.Options.Receiver.UserCommunications.Single(x => x.Type == context.Message.Options.Template.Communication);

        QueueNotificationHandlerGuard.Check(renderedTemplate, context.Message.Options, _logger);
        // send Push Notification using relevant SDK

        var pushSent = true;
        if (pushSent)
        {
            await context.RespondAsync<NotificationSent>(new
            {
                NotificationId = context.Message.NotificationId,
                Gateway = context.Message.Options.Template.Communication
            });
        }
        else
        {
            await context.RespondAsync<NotificationFailed>(new
            {
                NotificationId = context.Message.NotificationId,
                Gateway = context.Message.Options.Template.Communication
            });
        }
    }
}