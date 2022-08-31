using FooCommerce.Domain;

namespace FooCommerce.Application.Entities.Configurations;

public record Setting
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public string Key { get; init; }
    public string Value { get; init; }
}