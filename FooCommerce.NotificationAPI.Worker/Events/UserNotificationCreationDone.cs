namespace FooCommerce.NotificationAPI.Worker.Events;

public interface UserNotificationCreationDone
{
    Guid UserNotificationId { get; }
}