using FooCommerce.Domain.Entities;

namespace FooCommerce.Application.Entities.Membership;

public record User
    : IEntity
{
    public Guid Id { get; init; }
    public DateTimeOffset Created { get; init; }
    public byte[] RowVersion { get; init; }
    public Guid? ParentId { get; init; }
}