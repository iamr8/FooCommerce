using FooCommerce.Application.Notifications.Enums;

namespace FooCommerce.Application.Notifications.Publishers;

public record UpdateUserNotificationState(Guid UserNotificationId, UserNotificationUpdateState State);