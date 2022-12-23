using FooCommerce.Domain;

namespace FooCommerce.BasketService.DbProvider.Entities;

public record ShoppingCart
    : IEntity, IEntitySoftDeletable, IEntityExternalId
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; set; }
    public uint ExternalId { get; init; }
    public ushort Quantity { get; init; }
    public decimal Amount { get; init; }
    public Guid PurchasePriceId { get; init; }
}