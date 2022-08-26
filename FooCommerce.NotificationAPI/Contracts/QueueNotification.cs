using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.NotificationAPI.Contracts;

public interface QueueNotification : INotificationId
{
    INotificationOptions Options { get; }
}