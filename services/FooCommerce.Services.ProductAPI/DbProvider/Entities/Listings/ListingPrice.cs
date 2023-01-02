using FooCommerce.Domain;

namespace FooCommerce.CatalogService.DbProvider.Entities.Listings;

public record ListingPrice
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public decimal Amount { get; init; }
    public Guid ListingId { get; init; }
    public virtual Listing Listing { get; init; }
}