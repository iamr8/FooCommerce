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
    public string Name { get; init; }
    public Guid ProductId { get; init; }
    public Guid UserSubscriptionId { get; init; }
}