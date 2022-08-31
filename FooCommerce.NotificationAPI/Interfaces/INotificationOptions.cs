using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Models;

namespace FooCommerce.NotificationAPI.Interfaces;

public interface INotificationOptions : INotificationRequestInfo
{
    NotificationAction Action { get; }
    NotificationReceiverProvider Receiver { get; }
    IEnumerable<INotificationContent> Content { get; }
    IEnumerable<object> Bag { get; }
}