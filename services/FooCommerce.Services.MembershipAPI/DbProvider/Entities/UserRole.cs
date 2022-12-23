using FooCommerce.Domain;

namespace FooCommerce.MembershipService.DbProvider.Entities;

public record UserRole
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public Guid UserId { get; init; }
    public Guid RoleId { get; init; }
    public virtual User User { get; init; }
    public virtual Role Role { get; init; }
}