using FooCommerce.Application.HttpContextRequest;

namespace FooCommerce.Application.Notifications.Interfaces;

public interface INotificationRequestInfo
{
    HttpRequestInfo RequestInfo { get; }
}