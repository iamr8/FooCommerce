using FooCommerce.NotificationAPI.Enums;

namespace FooCommerce.NotificationAPI.Contracts;

public interface UpdateAuthTokenState
{
    Guid AuthTokenId { get; }
    UserNotificationUpdateState State { get; }
}