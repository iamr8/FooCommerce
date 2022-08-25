using FooCommerce.Domain.Interfaces.Database;

namespace FooCommerce.Application.Listings.Entities;

public record ListingRating
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public ushort Rate { get; init; }
    public Guid ListingId { get; init; }
    public Guid UserId { get; init; }
}