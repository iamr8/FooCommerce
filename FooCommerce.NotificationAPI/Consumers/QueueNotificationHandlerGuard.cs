using FooCommerce.NotificationAPI.Interfaces;

using Microsoft.Extensions.Logging;

namespace FooCommerce.NotificationAPI.Consumers;

internal static class QueueNotificationHandlerGuard
{
    public static void Check<TModel, TOptions, TLogger>(TModel model, TOptions options, ILogger<TLogger> _logger) where TOptions : INotificationCommunicationOptions
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