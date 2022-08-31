using FooCommerce.Domain;

namespace FooCommerce.NotificationAPI.Worker.Tests.Fakes.Entities;

public record User
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public Guid? ParentId { get; init; }
}