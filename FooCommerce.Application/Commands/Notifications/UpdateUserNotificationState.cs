using FooCommerce.Application.Enums.Notifications;

using MediatR;

namespace FooCommerce.Application.Commands.Notifications;

public record UpdateUserNotificationState(Guid UserNotificationId, UserNotificationUpdateState State) : INotification;