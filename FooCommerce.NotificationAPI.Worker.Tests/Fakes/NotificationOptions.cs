using FooCommerce.Core.HttpContextRequest;
using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Interfaces;
using FooCommerce.NotificationAPI.Models;

namespace FooCommerce.NotificationAPI.Worker.Tests.Fakes;

public record NotificationOptions : INotificationOptions
{
    public HttpRequestInfo RequestInfo { get; init; }
    public NotificationAction Action { get; init; }
    public NotificationReceiverProvider Receiver { get; init; }
    public IEnumerable<INotificationContent> Content { get; }
    public IEnumerable<object> Bag { get; }
}