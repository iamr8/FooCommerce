using FooCommerce.Domain.ContextRequest;
using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Interfaces;

namespace FooCommerce.NotificationAPI.Models;

public record NotificationOptions
    : INotificationOptions
{
    public ContextRequestInfo RequestInfo { get; init; }
    public Guid? UserId { get; }
    public NotificationAction Action { get; init; }
    public List<NotificationLink> Links { get; init; }
    public List<NotificationFormatter> Formatters { get; init; }
    public NotificationReceiverProvider ReceiverProvider { get; init; }
    public List<object> Bag { get; init; }
}