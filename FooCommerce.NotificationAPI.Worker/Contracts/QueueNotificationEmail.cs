using FooCommerce.NotificationAPI.Interfaces;
using FooCommerce.NotificationAPI.Models;
using FooCommerce.NotificationAPI.Models.Communications;

namespace FooCommerce.NotificationAPI.Worker.Contracts;

public interface QueueNotificationEmail
    : INotificationId, INotificationOptionsBase
{
    bool IsImportant { get; }
    NotificationReceiver Receiver { get; }
    NotificationEmailModel Model { get; }
}