using FooCommerce.NotificationAPI.Worker.Interfaces;

namespace FooCommerce.NotificationAPI.Worker.Dtos;

public record NotificationModel
    : INotification
{
    public Guid NotificationId { get; init; }
    public IReadOnlyList<INotificationTemplate> Templates { get; init; }
}