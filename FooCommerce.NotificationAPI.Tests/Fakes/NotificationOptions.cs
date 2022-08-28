using FooCommerce.Application;
using FooCommerce.Application.HttpContextRequest;
using FooCommerce.Application.Notifications.Enums;
using FooCommerce.Application.Notifications.Interfaces;
using FooCommerce.Application.Notifications.Models;

namespace FooCommerce.NotificationAPI.Tests.Fakes;

public record NotificationOptions : INotificationOptions
{
    public HttpRequestInfo RequestInfo { get; init; }
    public NotificationAction Action { get; init; }
    public NotificationReceiverProvider Receiver { get; init; }
    public IEnumerable<INotificationContent> Content { get; }
    public IEnumerable<object> Bag { get; }
}