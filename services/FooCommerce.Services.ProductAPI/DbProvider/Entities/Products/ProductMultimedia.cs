using FooCommerce.Domain;

namespace FooCommerce.CatalogService.DbProvider.Entities.Products;

public record ProductMultimedia
    : IEntity, IEntityExternalId, IEntityMedia, IEntitySortable
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public uint ExternalId { get; init; }
    public bool IsVideo { get; init; }
    public string Metadata { get; init; }
    public string Path { get; init; }
    public bool IsOriginal { get; init; }
    public int Order { get; set; }
    public Guid ProductId { get; init; }
    public virtual Product Product { get; init; }
}