using FooCommerce.Domain;

namespace FooCommerce.Services.ProductAPI.DbProvider.Entities.Products;

public record ProductCategoryGroup
    : IEntity, IEntitySoftDeletable, IEntityHideable, IEntityPublicId, IEntityCategory, IEntitySortable
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; init; }
    public uint PublicId { get; init; }
    public bool IsHidden { get; init; }
    public string Name { get; init; }
    public string Slug { get; init; }
    public string Description { get; init; }
    public string Icon { get; init; }
    public int Order { get; set; }
    public virtual ICollection<ProductCategory> Categories { get; init; }
}