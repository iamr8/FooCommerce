using FooCommerce.Application.Models;
using FooCommerce.Application.Notifications.Enums;

namespace FooCommerce.Application.Notifications.Interfaces;

public interface INotificationOptions : INotificationRequestInfo
{
    IEndUser RequestInfo { get; set; }
    NotificationAction Action { get; set; }
    INotificationReceiver Receiver { get; set; }
    IEnumerable<INotificationContent> Content { get; set; }
    IEnumerable<object> Bag { get; set; }
}