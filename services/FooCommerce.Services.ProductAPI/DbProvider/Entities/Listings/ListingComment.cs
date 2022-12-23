using FooCommerce.Domain;

namespace FooCommerce.Services.ProductAPI.DbProvider.Entities.Listings;

public record ListingComment
    : IEntity, IEntityExternalId
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public uint ExternalId { get; init; }
    public string Comment { get; init; }
    public Guid? CommentId { get; init; }
    public Guid ListingId { get; init; }
    public Guid UserId { get; set; }
}