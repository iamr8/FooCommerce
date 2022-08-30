using FooCommerce.Core.HttpContextRequest;

namespace FooCommerce.Core.Notifications.Interfaces;

public interface INotificationRequestInfo
{
    HttpRequestInfo RequestInfo { get; }
}