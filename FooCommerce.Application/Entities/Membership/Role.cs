using FooCommerce.Application.Enums.Membership;
using FooCommerce.Domain.Interfaces.Database;

namespace FooCommerce.Application.Entities.Membership;

public record Role
    : IEntity, IEntitySoftDeletable, IEntityHideable
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; init; }
    public bool IsHidden { get; init; }
    public RoleType Type { get; init; }
}