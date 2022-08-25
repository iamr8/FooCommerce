using FooCommerce.Application.Enums.Notifications;
using FooCommerce.Application.Models;

namespace FooCommerce.Application.Interfaces.Notifications;

public interface INotificationOptions : INotificationRequestInfo
{
    IEndUser RequestInfo { get; set; }
    NotificationAction Action { get; set; }
    INotificationReceiver Receiver { get; set; }
    IEnumerable<INotificationContent> Content { get; set; }
    IEnumerable<object> Bag { get; set; }
}