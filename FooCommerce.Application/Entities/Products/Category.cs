using FooCommerce.Domain;

namespace FooCommerce.Application.Entities.Products;

public record Category
    : IEntity, IEntitySoftDeletable, IEntityHideable, IEntityPublicId, IEntityCategory
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; init; }
    public uint PublicId { get; init; }
    public bool IsHidden { get; init; }
    public ushort Type { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string Icon { get; init; }
    public Guid ParentId { get; init; }
    public Guid TopCategoryId { get; init; }
}