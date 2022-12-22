using FooCommerce.Domain;

namespace FooCommerce.Services.ProductAPI.DbProvider.Entities.Listings.Entities;

public record ListingComment
    : IEntity, IEntityPublicId
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public uint PublicId { get; init; }
    public string Comment { get; init; }
    public Guid? CommentId { get; init; }
    public Guid ListingId { get; init; }
    public Guid UserId { get; set; }
}