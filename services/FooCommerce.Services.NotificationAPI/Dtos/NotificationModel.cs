using FooCommerce.NotificationService.Interfaces;

namespace FooCommerce.NotificationService.Dtos;

public record NotificationModel
    : INotification
{
    public Guid NotificationId { get; init; }
    public IEnumerable<ITemplate> Templates { get; init; }
}