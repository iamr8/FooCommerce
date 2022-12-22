#nullable enable

using FooCommerce.Domain;

namespace FooCommerce.DbProvider.Entities.Listings.Entities;

public record ListingReport
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public ushort Reason { get; init; }
    public string? Description { get; init; }
    public Guid ListingId { get; init; }
    public Guid UserId { get; init; }
}