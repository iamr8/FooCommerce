using FooCommerce.Domain;

namespace FooCommerce.DbProvider.Entities.Identities;

public record Role
    : IEntity, IEntitySoftDeletable, IEntityHideable
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; init; }
    public bool IsHidden { get; init; }
    public byte Type { get; init; } // RoleType
}