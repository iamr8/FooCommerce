#nullable enable

using FooCommerce;
using FooCommerce.Domain;

namespace FooCommerce.Application.DbProvider.Entities.Products;

public record Product
    : IEntity, IEntitySoftDeletable
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; init; }
    public string? Name { get; init; }
    public Guid CategoryId { get; init; }
}