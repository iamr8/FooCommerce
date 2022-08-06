using FooCommerce.Domain.Entities;

namespace FooCommerce.Application.Entities.Listings;

public record ListingComment
    : IEntity, IEntityPublicId
{
    public Guid Id { get; init; }
    public DateTimeOffset Created { get; init; }
    public byte[] RowVersion { get; init; }
    public uint PublicId { get; init; }
    public string Comment { get; init; }
    public Guid? CommentId { get; init; }
    public Guid ListingId { get; init; }
    public Guid UserId { get; set; }
}