using FooCommerce.Domain;

namespace FooCommerce.CatalogService.DbProvider.Entities.Products;

public record Specification
    : IEntity, IEntitySoftDeletable, IEntityVisibility
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; set; }
    public bool IsInvisible { get; set; }
    public string Name { get; init; }
    public virtual ICollection<ProductSpecification> ProductSpecifications { get; init; }
}