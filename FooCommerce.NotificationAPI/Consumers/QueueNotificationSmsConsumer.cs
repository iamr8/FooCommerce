using FooCommerce.Application.Membership.Entities;
using FooCommerce.Application.Membership.Publishers;
using FooCommerce.Application.Notifications.Dtos;
using FooCommerce.Application.Notifications.Enums;
using FooCommerce.NotificationAPI.Contracts;
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
        var renderedTemplate = context.Message.Options.Factory.CreateSmsModel(
            (NotificationTemplateSmsModel)context.Message.Options.Template,
            options =>
            {
                options.WebsiteUrl = context.Message.Options.WebsiteUrl;
            });
        var receiver = context.Message.Options.Options.Receiver.UserCommunications.Single(x => x.Type == context.Message.Options.Template.Communication);

        QueueNotificationHandlerGuard.Check(renderedTemplate, context.Message.Options, _logger);

        // TODO: SDK must be implemented
        var smsSent = true;
        if (smsSent)
        {
            await context.RespondAsync<NotificationSent>(new
            {
                NotificationId = context.Message.NotificationId,
                Gateway = context.Message.Options.Template.Communication
            });

            var token = context.Message.Options.Options.Bag.OfType<AuthToken>().FirstOrDefault();
            if (token != null)
            {
                await context.Publish(new UpdateAuthTokenState(token.Id, UserNotificationUpdateState.Sent), context.CancellationToken);
            }
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