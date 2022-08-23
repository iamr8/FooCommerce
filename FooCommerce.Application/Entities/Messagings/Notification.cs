using FooCommerce.Application.Enums.Notifications;
using FooCommerce.Domain.Interfaces.Database;

namespace FooCommerce.Application.Entities.Messagings;

public record Notification
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public NotificationAction Action { get; init; }
}