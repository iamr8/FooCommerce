using FooCommerce.Application.Enums.Notifications;

namespace FooCommerce.Application.Publishers.Notifications;

public record UpdateUserNotificationState(Guid UserNotificationId, UserNotificationUpdateState State);