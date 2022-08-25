using FooCommerce.Domain.Interfaces.Database;

namespace FooCommerce.Application.Membership.Entities;

public record User
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public Guid? ParentId { get; init; }
}