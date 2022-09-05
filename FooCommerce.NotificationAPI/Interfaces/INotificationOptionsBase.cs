using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Models;

namespace FooCommerce.NotificationAPI.Interfaces;

public interface INotificationOptionsBase : INotificationRequestInfo
{
    Guid? UserId { get; }
    NotificationAction Action { get; }
    List<NotificationLink> Links { get; }
    List<NotificationFormatter> Formatters { get; }
    List<object> Bag { get; }
}