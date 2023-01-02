using System.Collections;
using FooCommerce.Domain;

namespace FooCommerce.CatalogService.DbProvider.Entities.Listings;

public record ListingComment
    : IEntity, IEntityExternalId
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public uint ExternalId { get; init; }
    public string Comment { get; init; }
    public Guid? ParentCommentId { get; init; }
    public Guid ListingId { get; init; }
    public Guid UserId { get; set; }
    public virtual Listing Listing { get; init; }
    public virtual ListingComment ParentComment { get; init; }
    public virtual ICollection<ListingComment> ChildComments { get; init; }

}