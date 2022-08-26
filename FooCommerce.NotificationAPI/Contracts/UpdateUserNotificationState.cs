using FooCommerce.NotificationAPI.Enums;

namespace FooCommerce.NotificationAPI.Contracts;

public interface UpdateUserNotificationState
{
    Guid UserNotificationId { get; }
    UserNotificationUpdateState State { get; }
}