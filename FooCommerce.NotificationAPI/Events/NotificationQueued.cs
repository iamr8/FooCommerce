using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.NotificationAPI.Events;

public interface NotificationQueued
    : INotificationId
{
}