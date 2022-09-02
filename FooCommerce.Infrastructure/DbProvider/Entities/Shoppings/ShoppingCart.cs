using FooCommerce.Domain;

namespace FooCommerce.Infrastructure.DbProvider.Entities.Shoppings;

public record ShoppingCart
    : IEntity, IEntitySoftDeletable, IEntityPublicId
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; init; }
    public uint PublicId { get; init; }
    public ushort Quantity { get; init; }
    public decimal Amount { get; init; }
    public Guid PurchasePriceId { get; init; }
}