namespace FooCommerce.Services.NotificationAPI.Interfaces;

public interface INotification
{
    Guid NotificationId { get; }
    IEnumerable<ITemplate> Templates { get; }
}