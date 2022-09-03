namespace FooCommerce.NotificationAPI.Worker.Contracts;

public interface CreateUserNotification
{
    string Output { get; }
    DateTime Sent { get; }
    Guid NotificationId { get; }
    Guid UserId { get; }
}