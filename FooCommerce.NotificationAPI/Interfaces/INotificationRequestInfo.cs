using FooCommerce.Common.HttpContextRequest;

namespace FooCommerce.NotificationAPI.Interfaces;

public interface INotificationRequestInfo
{
    HttpRequestInfo RequestInfo { get; }
}