using FooCommerce.Application.Enums.Notifications;

namespace FooCommerce.Application.Publishers.Membership;

public record UpdateAuthTokenState(Guid AuthTokenId, UserNotificationUpdateState State);