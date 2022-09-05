using FooCommerce.Domain;

namespace FooCommerce.DbProvider.Entities.Identities;

public record UserRole
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public Guid UserId { get; init; }
    public Guid RoleId { get; init; }
}