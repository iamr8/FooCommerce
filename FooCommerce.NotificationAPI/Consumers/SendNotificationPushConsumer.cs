using FooCommerce.Application.Notifications.Dtos;
using FooCommerce.NotificationAPI.Publishers;

using MassTransit;

using Microsoft.Extensions.Logging;

namespace FooCommerce.NotificationAPI.Consumers;

public class SendNotificationPushConsumer : IConsumer<SendNotificationPush>
{
    private readonly ILogger<SendNotificationPushConsumer> _logger;

    public SendNotificationPushConsumer(ILogger<SendNotificationPushConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<SendNotificationPush> context)
    {
        var renderedTemplate = context.Message.Options.Factory.CreatePushModel(
            (NotificationTemplatePushModel)context.Message.Options.Template,
            options =>
            {
                options.WebsiteUrl = context.Message.Options.WebsiteUrl;
            });
        var receiver = context.Message.Options.Options.Receiver.UserCommunications.Single(x => x.Type == context.Message.Options.Template.Communication);

        SendNotificationHandlerGuard.Check(renderedTemplate, context.Message.Options, _logger);
        // send Push Notification using relevant SDK

        return Task.FromResult(0);
    }
}