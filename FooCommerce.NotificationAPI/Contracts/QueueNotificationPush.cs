using FooCommerce.Application.Notifications.Interfaces;
using FooCommerce.NotificationAPI.Models.Types;

namespace FooCommerce.NotificationAPI.Contracts;

public interface QueueNotificationPush
    : INotificationId, INotificationOptions
{
    NotificationPushModel Model { get; }
}