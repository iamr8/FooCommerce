using FooCommerce.Domain;

namespace FooCommerce.Application.DbProvider.Entities.Shoppings;

public record Payment
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public decimal Amount { get; init; }
    public ushort Currency { get; init; }
    public ushort Gateway { get; init; }
    public ushort Method { get; init; }
    public ushort Status { get; init; }
    public string Details { get; init; }
    public string Reference { get; init; }
    public string TransactionId { get; init; }
    public Guid CheckoutId { get; init; }
}