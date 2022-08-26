using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.Application.Notifications.Contracts;

public interface QueueNotification : INotificationId, INotificationOptions
{
}