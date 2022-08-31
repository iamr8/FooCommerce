using FooCommerce.Domain;

namespace FooCommerce.Application.Entities.Products;

public record Specification
    : IEntity, IEntitySoftDeletable, IEntityHideable
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; init; }
    public bool IsHidden { get; init; }
    public string Name { get; init; }
}