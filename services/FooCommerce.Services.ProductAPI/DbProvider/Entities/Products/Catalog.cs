using FooCommerce.Domain;

namespace FooCommerce.Services.ProductAPI.DbProvider.Entities.Products;

public record Catalog
    : IEntity, IEntitySoftDeletable, IEntityVisibility, IEntityExternalId, IEntitySortable
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; set; }
    public uint ExternalId { get; init; }
    public bool IsInvisible { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public int Order { get; set; }
    public Guid? ParentId { get; set; }
    public virtual Catalog Parent { get; init; }
    public virtual ICollection<Catalog> Children { get; init; }
    public virtual ICollection<Product> Products { get; init; }
}