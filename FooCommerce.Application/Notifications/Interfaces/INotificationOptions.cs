using FooCommerce.Application.Notifications.Enums;

namespace FooCommerce.Application.Notifications.Interfaces;

public interface INotificationOptions : INotificationRequestInfo
{
    NotificationAction Action { get; set; }
    INotificationReceiver Receiver { get; set; }
    IEnumerable<INotificationContent> Content { get; set; }
    IEnumerable<object> Bag { get; set; }
}