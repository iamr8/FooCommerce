using FooCommerce.Application.Interfaces.Notifications;

using Microsoft.Extensions.Logging;

namespace FooCommerce.NotificationAPI.Commands;

internal static class SendNotificationHandlerGuard
{
    public static void Check<T1, T2, T3>(T1 model, T2 options, ILogger<T3> _logger) where T2 : INotificationSendToCommunicationOptions
    {
        if (model == null)
        {
            _logger.LogError("Action {0} needs to send notification via {1}, but unable to create appropriate email template for it.", options.Options.Action, options.Template.Communication);
            return;
        }

        var receiver = options.Options.Receiver.UserCommunications.SingleOrDefault(x => x.Type == options.Template.Communication)?.Value;
        if (string.IsNullOrEmpty(receiver))
        {
            _logger.LogError("Action {0} needs to send notification via email, but unable to create appropriate email template for it.", options.Options.Action);
            return;
        }
    }
}