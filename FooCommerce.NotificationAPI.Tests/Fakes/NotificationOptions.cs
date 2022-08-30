using FooCommerce.Application;
using FooCommerce.Application.Notifications.Enums;
using FooCommerce.Core.HttpContextRequest;
using FooCommerce.Core.Notifications.Interfaces;
using FooCommerce.Core.Notifications.Models;

namespace FooCommerce.NotificationAPI.Tests.Fakes;

public record NotificationOptions : INotificationOptions
{
    public HttpRequestInfo RequestInfo { get; init; }
    public NotificationAction Action { get; init; }
    public NotificationReceiverProvider Receiver { get; init; }
    public IEnumerable<INotificationContent> Content { get; }
    public IEnumerable<object> Bag { get; }
}