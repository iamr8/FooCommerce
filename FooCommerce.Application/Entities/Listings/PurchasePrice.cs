using FooCommerce.Domain.Entities;

namespace FooCommerce.Application.Entities.Listings;

public record PurchasePrice
    : IEntity
{
    public Guid Id { get; init; }
    public DateTimeOffset Created { get; init; }
    public byte[] RowVersion { get; init; }
    public decimal Amount { get; init; }
    public Guid ListingId { get; init; }
}