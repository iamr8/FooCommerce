using FooCommerce.Application.Enums.Notifications;

using MediatR;

namespace FooCommerce.Application.Commands.Membership;

public record UpdateAuthTokenState(Guid AuthTokenId, UserNotificationUpdateState State) : INotification;