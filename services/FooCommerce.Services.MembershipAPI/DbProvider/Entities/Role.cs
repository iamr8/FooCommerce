using FooCommerce.Domain;
using FooCommerce.Services.MembershipAPI.Enums;

namespace FooCommerce.Services.MembershipAPI.DbProvider.Entities;

public record Role
    : IEntity, IEntitySoftDeletable, IEntityVisibility
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; set; }
    public bool IsInvisible { get; set; }
    public RoleType Type { get; init; }
    public virtual ICollection<UserRole> UserRoles { get; init; }
}