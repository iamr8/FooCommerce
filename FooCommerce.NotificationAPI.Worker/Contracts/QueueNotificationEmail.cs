using FooCommerce.NotificationAPI.Interfaces;
using FooCommerce.NotificationAPI.Worker.Models.Communications;

namespace FooCommerce.NotificationAPI.Worker.Contracts;

public interface QueueNotificationEmail
    : INotificationId, INotificationOptions
{
    bool IsImportant { get; }
    NotificationEmailModel Model { get; }
}