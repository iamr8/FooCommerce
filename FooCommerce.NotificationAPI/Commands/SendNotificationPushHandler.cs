using FooCommerce.Application.Commands.Notifications;
using FooCommerce.Application.Dtos.Notifications;

using MediatR;

using Microsoft.Extensions.Logging;

namespace FooCommerce.NotificationAPI.Commands;

public class SendNotificationPushHandler : INotificationHandler<SendNotificationPush>
{
    private readonly ILogger<SendNotificationPushHandler> _logger;

    public SendNotificationPushHandler(ILogger<SendNotificationPushHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SendNotificationPush notification, CancellationToken cancellationToken)
    {
        var renderedTemplate = notification.Options.Factory.CreatePushModel(
            (NotificationTemplatePushModel)notification.Options.Template,
            options =>
            {
                options.WebsiteUrl = notification.Options.WebsiteUrl;
            });
        var receiver = notification.Options.Options.Receiver.UserCommunications.Single(x => x.Type == notification.Options.Template.Communication);

        SendNotificationHandlerGuard.Check(renderedTemplate, notification.Options, _logger);
        // send Push Notification using relevant SDK

        return Task.FromResult(0);
    }
}