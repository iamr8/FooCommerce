#nullable enable

using FooCommerce.Domain;

namespace FooCommerce.Services.ProductAPI.DbProvider.Entities.Products;

public record Product
    : IEntity, IEntitySoftDeletable
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; init; }
    public string? Name { get; init; }
    public Guid CategoryId { get; init; }
    public virtual ProductCategory Category { get; init; }
    public virtual ICollection<ProductMultimedia> Multimedias { get; init; }
    public virtual ICollection<ProductSpecification> Specifications { get; init; }
}