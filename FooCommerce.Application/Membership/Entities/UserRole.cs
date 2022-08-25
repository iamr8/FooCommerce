using FooCommerce.Domain.Interfaces.Database;

namespace FooCommerce.Application.Membership.Entities;

public record UserRole
    : IEntity
{
    public UserRole()
    {
    }

    public UserRole(Guid roleId, Guid userId)
    {
        RoleId = roleId;
        UserId = userId;
    }

    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public Guid UserId { get; init; }
    public Guid RoleId { get; init; }
}