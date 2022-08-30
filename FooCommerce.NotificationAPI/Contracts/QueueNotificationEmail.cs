using FooCommerce.Core.Notifications.Interfaces;
using FooCommerce.NotificationAPI.Models.Types;

namespace FooCommerce.NotificationAPI.Contracts;

public interface QueueNotificationEmail
    : INotificationId, INotificationOptions
{
    bool IsImportant { get; }
    NotificationEmailModel Model { get; }
}