using FooCommerce.NotificationAPI.Bridge.Enums;
using FooCommerce.NotificationAPI.Bridge.Models;

namespace FooCommerce.NotificationAPI.Bridge.Interfaces;

public interface INotificationOptions : INotificationRequestInfo
{
    NotificationAction Action { get; }
    NotificationReceiverProvider Receiver { get; }
    IEnumerable<INotificationContent> Content { get; }
    IEnumerable<object> Bag { get; }
}