using FooCommerce.Application.Enums.Membership;
using FooCommerce.Domain.Entities;

namespace FooCommerce.Application.Entities.Membership;

public record Role
    : IEntity, IEntitySoftDeletable, IEntityPublicId, IEntityHideable
{
    public Guid Id { get; init; }
    public DateTimeOffset Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; init; }
    public uint PublicId { get; init; }
    public bool IsHidden { get; init; }
    public RoleTypes Type { get; init; }
}