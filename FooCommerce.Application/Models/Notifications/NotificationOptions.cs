using FooCommerce.Application.Enums.Notifications;
using FooCommerce.Application.Interfaces;
using FooCommerce.Application.Interfaces.Notifications;

namespace FooCommerce.Application.Models.Notifications;

public class NotificationOptions : INotificationOptions
{
    public IEndUser RequestInfo { get; set; }
    public NotificationAction Action { get; set; }

    public INotificationReceiver Receiver { get; set; }

    public IEnumerable<INotificationContent> Content { get; set; } = new List<INotificationContent>();

    public IEnumerable<object> Bag { get; set; } = new List<object>();
}