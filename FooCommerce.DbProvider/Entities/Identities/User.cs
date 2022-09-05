using FooCommerce.Domain;

namespace FooCommerce.DbProvider.Entities.Identities;

public record User
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public Guid? ParentId { get; init; }
}