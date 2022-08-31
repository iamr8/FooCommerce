using FooCommerce.Core.HttpContextRequest;

namespace FooCommerce.NotificationAPI.Interfaces;

public interface INotificationRequestInfo
{
    HttpRequestInfo RequestInfo { get; }
}