using FooCommerce.Application.Notifications.Dtos;
using FooCommerce.NotificationAPI.Publishers;

using MassTransit;

using Microsoft.Extensions.Logging;

namespace FooCommerce.NotificationAPI.Consumers;

public class SendNotificationPushInAppConsumer : IConsumer<SendNotificationPushInApp>
{
    private readonly ILogger<SendNotificationPushInAppConsumer> _logger;

    public SendNotificationPushInAppConsumer(ILogger<SendNotificationPushInAppConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<SendNotificationPushInApp> context)
    {
        var renderedTemplate = context.Message.Options.Factory.CreatePushModel(
            (NotificationTemplatePushModel)context.Message.Options.Template,
            options =>
            {
                options.WebsiteUrl = context.Message.Options.WebsiteUrl;
            });
        var receiver = context.Message.Options.Options.Receiver.UserCommunications.Single(x => x.Type == context.Message.Options.Template.Communication);

        SendNotificationHandlerGuard.Check(renderedTemplate, context.Message.Options, _logger);

        // save user context.Message in database

        return Task.FromResult(0);
    }
}