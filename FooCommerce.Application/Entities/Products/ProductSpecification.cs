#nullable enable

using FooCommerce;
using FooCommerce.Domain.Interfaces.Database;

namespace FooCommerce.Application.Entities.Products;

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
}