using FooCommerce.Application.Notifications.Interfaces;
using FooCommerce.NotificationAPI.Models.Types;

namespace FooCommerce.NotificationAPI.Contracts;

public interface QueueNotificationPushInApp
    : INotificationId, INotificationOptions
{
    NotificationPushInAppModel Model { get; }
}