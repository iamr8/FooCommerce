using FooCommerce.NotificationAPI.Interfaces;
using FooCommerce.NotificationAPI.Worker.Models.Communications;

namespace FooCommerce.NotificationAPI.Worker.Contracts;

public interface QueueNotificationPush
    : INotificationId, INotificationOptions
{
    NotificationPushModel Model { get; }
}