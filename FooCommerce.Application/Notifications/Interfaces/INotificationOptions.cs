using FooCommerce.Application.Notifications.Enums;
using FooCommerce.Application.Notifications.Models;

namespace FooCommerce.Application.Notifications.Interfaces;

public interface INotificationOptions : INotificationRequestInfo
{
    NotificationAction Action { get; }
    NotificationReceiverProvider Receiver { get; }
    IEnumerable<INotificationContent> Content { get; }
    IEnumerable<object> Bag { get; }
}