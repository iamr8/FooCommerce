using FooCommerce.NotificationAPI.Bridge.Interfaces;
using FooCommerce.NotificationAPI.Models.Types;

namespace FooCommerce.NotificationAPI.Contracts;

public interface QueueNotificationSms
    : INotificationId, INotificationOptions
{
    NotificationSmsModel Model { get; }
}