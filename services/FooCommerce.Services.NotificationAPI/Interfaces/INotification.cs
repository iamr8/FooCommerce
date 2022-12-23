namespace FooCommerce.NotificationService.Interfaces;

public interface INotification
{
    Guid NotificationId { get; }
    IEnumerable<ITemplate> Templates { get; }
}