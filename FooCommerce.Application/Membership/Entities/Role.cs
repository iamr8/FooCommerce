using FooCommerce.Application.Membership.Enums;
using FooCommerce.Domain;

namespace FooCommerce.Application.Membership.Entities;

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