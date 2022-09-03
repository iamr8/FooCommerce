using FooCommerce.Common.HttpContextRequest;
using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Interfaces;

namespace FooCommerce.NotificationAPI.Models;

public record NotificationOptions
    : INotificationOptions
{
    public HttpRequestInfo RequestInfo { get; init; }
    public Guid? UserId { get; }
    public NotificationAction Action { get; init; }
    public IEnumerable<NotificationLink> Links { get; init; }
    public IEnumerable<NotificationFormatter> Formatters { get; init; }
    public NotificationReceiverProvider ReceiverProvider { get; init; }
    public IEnumerable<object> Bag { get; init; }
}