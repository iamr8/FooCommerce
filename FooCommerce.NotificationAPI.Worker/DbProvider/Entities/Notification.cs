using FooCommerce.Domain;
using FooCommerce.NotificationAPI.Enums;

namespace FooCommerce.NotificationAPI.Worker.DbProvider.Entities;

public record Notification
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public NotificationAction Action { get; init; }
}