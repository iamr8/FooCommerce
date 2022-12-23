#nullable enable

using FooCommerce.Domain;

namespace FooCommerce.Services.ProductAPI.DbProvider.Entities.Products;

public record Product
    : IEntity, IEntitySoftDeletable
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; set; }
    public string? Name { get; init; }
    public Guid CatalogId { get; init; }
    public virtual Catalog Catalog { get; init; }
    public virtual ICollection<ProductMultimedia> Multimedias { get; init; }
    public virtual ICollection<ProductSpecification> Specifications { get; init; }
}