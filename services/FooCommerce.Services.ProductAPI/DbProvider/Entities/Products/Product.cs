#nullable enable

using System.Collections;
using FooCommerce.CatalogService.DbProvider.Entities.Listings;
using FooCommerce.Domain;

namespace FooCommerce.CatalogService.DbProvider.Entities.Products;

public record Product
    : IEntity, IEntitySoftDeletable, IEntityExternalId
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; set; }
    public uint ExternalId { get; init; }
    public string? Name { get; init; }
    public Guid CatalogId { get; init; }
    public virtual Catalog Catalog { get; init; }
    public virtual ICollection<ProductMedia> Medias { get; init; }
    public virtual ICollection<ProductSpecification> Specifications { get; init; }
    public virtual ICollection<Listing> Listings { get; init; }
}