using FooCommerce.NotificationAPI.Interfaces;
using FooCommerce.NotificationAPI.Models;
using FooCommerce.NotificationAPI.Models.Communications;

namespace FooCommerce.NotificationAPI.Contracts;

public interface QueueNotificationPushInApp
    : INotificationId, INotificationOptionsBase
{
    NotificationReceiver Receiver { get; }
    NotificationPushInAppModel Model { get; }
}