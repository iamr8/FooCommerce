using FooCommerce.Application.Notifications.Dtos;
using FooCommerce.NotificationAPI.Contracts;
using FooCommerce.NotificationAPI.Events;

using MassTransit;

using Microsoft.Extensions.Logging;

namespace FooCommerce.NotificationAPI.Consumers;

public class QueueNotificationPushInAppConsumer : IConsumer<QueueNotificationPushInApp>
{
    private readonly ILogger<QueueNotificationPushInAppConsumer> _logger;

    public QueueNotificationPushInAppConsumer(ILogger<QueueNotificationPushInAppConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<QueueNotificationPushInApp> context)
    {
        var renderedTemplate = context.Message.Options.Factory.CreatePushModel(
            (NotificationTemplatePushModel)context.Message.Options.Template,
            options =>
            {
                options.WebsiteUrl = context.Message.Options.WebsiteUrl;
            });
        var receiver = context.Message.Options.Options.Receiver.UserCommunications.Single(x => x.Type == context.Message.Options.Template.Communication);

        QueueNotificationHandlerGuard.Check(renderedTemplate, context.Message.Options, _logger);

        // save user context.Message in database
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