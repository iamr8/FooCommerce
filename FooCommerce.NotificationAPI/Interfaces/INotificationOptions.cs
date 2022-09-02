using FooCommerce.NotificationAPI.Models;

namespace FooCommerce.NotificationAPI.Interfaces;

public interface INotificationOptions : INotificationOptionsBase
{
    NotificationReceiverProvider ReceiverProvider { get; }
}