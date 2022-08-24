using FooCommerce.Application.Dtos.Notifications;
using FooCommerce.NotificationAPI.Commands;

using MediatR;

using Microsoft.Extensions.Logging;

namespace FooCommerce.NotificationAPI.CommandsHandlers;

public class SendNotificationPushInAppHandler : INotificationHandler<SendNotificationPushInApp>
{
    private readonly ILogger<SendNotificationPushInAppHandler> _logger;

    public SendNotificationPushInAppHandler(ILogger<SendNotificationPushInAppHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SendNotificationPushInApp notification, CancellationToken cancellationToken)
    {
        var renderedTemplate = notification.Options.Factory.CreatePushModel(
            (NotificationTemplatePushModel)notification.Options.Template,
            options =>
            {
                options.WebsiteUrl = notification.Options.WebsiteUrl;
            });
        var receiver = notification.Options.Options.Receiver.UserCommunications.Single(x => x.Type == notification.Options.Template.Communication);

        SendNotificationHandlerGuard.Check(renderedTemplate, notification.Options, _logger);

        // save user notification in database

        return Task.FromResult(0);
    }
}