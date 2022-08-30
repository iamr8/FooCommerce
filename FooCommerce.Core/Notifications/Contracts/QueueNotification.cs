using FooCommerce.Core.Notifications.Interfaces;

namespace FooCommerce.Core.Notifications.Contracts;

public interface QueueNotification
    : INotificationId, INotificationOptions
{
}