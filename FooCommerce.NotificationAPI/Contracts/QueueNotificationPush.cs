using FooCommerce.Application.Notifications.Models.Options;

namespace FooCommerce.NotificationAPI.Contracts;

public interface QueueNotificationPush : INotificationId
{
    SendNotificationPushOptions Options { get; }
}