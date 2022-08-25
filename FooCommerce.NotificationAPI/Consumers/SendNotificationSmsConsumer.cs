using FooCommerce.Application.Membership.Entities;
using FooCommerce.Application.Membership.Publishers;
using FooCommerce.Application.Notifications.Dtos;
using FooCommerce.Application.Notifications.Enums;
using FooCommerce.NotificationAPI.Publishers;

using MassTransit;

using Microsoft.Extensions.Logging;

namespace FooCommerce.NotificationAPI.Consumers;

public class SendNotificationSmsConsumer : IConsumer<SendNotificationSms>
{
    private readonly ILogger<SendNotificationSmsConsumer> _logger;

    public SendNotificationSmsConsumer(ILogger<SendNotificationSmsConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendNotificationSms> context)
    {
        var renderedTemplate = context.Message.Options.Factory.CreateSmsModel(
            (NotificationTemplateSmsModel)context.Message.Options.Template,
            options =>
            {
                options.WebsiteUrl = context.Message.Options.WebsiteUrl;
            });
        var receiver = context.Message.Options.Options.Receiver.UserCommunications.Single(x => x.Type == context.Message.Options.Template.Communication);

        SendNotificationHandlerGuard.Check(renderedTemplate, context.Message.Options, _logger);

        // TODO: SDK must be implemented
        var smsSent = true;
        if (smsSent)
        {
            var token = context.Message.Options.Options.Bag.OfType<AuthToken>().FirstOrDefault();
            if (token != null)
            {
                await context.Publish(new UpdateAuthTokenState(token.Id, UserNotificationUpdateState.Sent), context.CancellationToken);
            }
        }
    }
}