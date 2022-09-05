using FooCommerce.Domain.ContextRequest;

namespace FooCommerce.NotificationAPI.Interfaces;

public interface INotificationRequestInfo
{
    ContextRequestInfo RequestInfo { get; }
}