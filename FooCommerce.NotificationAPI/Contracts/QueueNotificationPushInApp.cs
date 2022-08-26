using FooCommerce.Application.Notifications.Interfaces;
using FooCommerce.NotificationAPI.Interfaces;

namespace FooCommerce.NotificationAPI.Contracts;

public interface QueueNotificationPushInApp
    : INotificationId, INotificationCommunicationOptions
{
}