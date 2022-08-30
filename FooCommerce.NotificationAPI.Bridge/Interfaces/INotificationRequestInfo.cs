using FooCommerce.Core.HttpContextRequest;

namespace FooCommerce.NotificationAPI.Bridge.Interfaces;

public interface INotificationRequestInfo
{
    HttpRequestInfo RequestInfo { get; }
}