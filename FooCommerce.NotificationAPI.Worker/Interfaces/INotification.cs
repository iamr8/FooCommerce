namespace FooCommerce.NotificationAPI.Worker.Interfaces;

public interface INotification
{
    Guid NotificationId { get; }
    IReadOnlyList<INotificationTemplate> Templates { get; }
}