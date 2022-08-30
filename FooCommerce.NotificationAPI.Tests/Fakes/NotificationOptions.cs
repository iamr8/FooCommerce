using FooCommerce.Core.HttpContextRequest;
using FooCommerce.NotificationAPI.Bridge.Enums;
using FooCommerce.NotificationAPI.Bridge.Interfaces;
using FooCommerce.NotificationAPI.Bridge.Models;

namespace FooCommerce.NotificationAPI.Tests.Fakes;

public record NotificationOptions : INotificationOptions
{
    public HttpRequestInfo RequestInfo { get; init; }
    public NotificationAction Action { get; init; }
    public NotificationReceiverProvider Receiver { get; init; }
    public IEnumerable<INotificationContent> Content { get; }
    public IEnumerable<object> Bag { get; }
}