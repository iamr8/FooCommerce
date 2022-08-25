using FooCommerce.Application.Notifications.Enums;

namespace FooCommerce.Application.Membership.Publishers;

public record UpdateAuthTokenState(Guid AuthTokenId, UserNotificationUpdateState State);