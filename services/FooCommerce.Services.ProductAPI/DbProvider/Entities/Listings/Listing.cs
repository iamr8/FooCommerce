using FooCommerce.CatalogService.DbProvider.Entities.Products;
using FooCommerce.Domain;

namespace FooCommerce.CatalogService.DbProvider.Entities.Listings;

public record Listing
    : IEntity, IEntityExternalId
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public uint ExternalId { get; init; }
    public bool IsSuspended { get; init; }
    public bool IsCancelled { get; init; }

    /// <summary>
    /// Could be used for second-handed products
    /// </summary>
    public string Name { get; init; }
    public Guid ProductId { get; init; }
    public virtual Product Product { get; init; }
    public virtual ICollection<ListingComment> Comments { get; init; }
    public virtual ICollection<ListingLike> Likes { get; init; }
    public virtual ICollection<ListingRating> Ratings { get; init; }
    public virtual ICollection<ListingReport> Reports { get; init; }
    public virtual ICollection<ListingPrice> Prices { get; init; }
}