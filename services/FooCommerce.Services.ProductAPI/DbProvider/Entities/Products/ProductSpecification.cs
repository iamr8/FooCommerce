#nullable enable

using FooCommerce.Domain;

namespace FooCommerce.Services.ProductAPI.DbProvider.Entities.Products;

public record ProductSpecification
    : IEntity, IEntitySortable
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public string? Value { get; init; }
    public int Order { get; set; }
    public Guid SpecificationId { get; init; }
    public Guid ProductId { get; init; }
    public virtual Product Product { get; init; }
    public virtual Specification Specification { get; init; }
}