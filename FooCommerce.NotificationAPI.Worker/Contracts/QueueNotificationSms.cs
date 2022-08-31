using FooCommerce.NotificationAPI.Interfaces;
using FooCommerce.NotificationAPI.Worker.Models.Communications;

namespace FooCommerce.NotificationAPI.Worker.Contracts;

public interface QueueNotificationSms
    : INotificationId, INotificationOptions
{
    NotificationSmsModel Model { get; }
}