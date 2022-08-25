using FooCommerce.Application.Notifications.Enums;
using FooCommerce.Domain.Interfaces.Database;

namespace FooCommerce.Application.Notifications.Entities;

public record Notification
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public NotificationAction Action { get; init; }
}