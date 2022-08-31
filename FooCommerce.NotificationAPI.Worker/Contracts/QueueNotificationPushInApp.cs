using FooCommerce.NotificationAPI.Interfaces;
using FooCommerce.NotificationAPI.Worker.Models.Communications;

namespace FooCommerce.NotificationAPI.Worker.Contracts;

public interface QueueNotificationPushInApp
    : INotificationId, INotificationOptions
{
    NotificationPushInAppModel Model { get; }
}