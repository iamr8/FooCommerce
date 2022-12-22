using FooCommerce.Services.NotificationAPI.Interfaces;

namespace FooCommerce.Services.NotificationAPI.Dtos;

public record NotificationModel
    : INotification
{
    public Guid NotificationId { get; init; }
    public IEnumerable<ITemplate> Templates { get; init; }
}