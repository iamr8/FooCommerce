using FooCommerce.NotificationAPI.Bridge.Interfaces;

namespace FooCommerce.NotificationAPI.Bridge.Contracts;

public interface QueueNotification
    : INotificationId, INotificationOptions
{
}