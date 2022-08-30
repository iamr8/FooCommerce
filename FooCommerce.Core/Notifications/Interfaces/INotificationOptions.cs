using FooCommerce.Application.Notifications.Enums;
using FooCommerce.Core.Notifications.Models;

namespace FooCommerce.Core.Notifications.Interfaces;

public interface INotificationOptions : INotificationRequestInfo
{
    NotificationAction Action { get; }
    NotificationReceiverProvider Receiver { get; }
    IEnumerable<INotificationContent> Content { get; }
    IEnumerable<object> Bag { get; }
}