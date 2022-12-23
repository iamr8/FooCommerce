using FooCommerce.Domain;

namespace FooCommerce.Services.BasketAPI.DbProvider.Entities;

public record ShoppingBasket
    : IEntity, IEntitySoftDeletable, IEntityExternalId
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; set; }
    public uint ExternalId { get; init; }
    public Guid TopCategoryId { get; init; }
    public Guid UserSubscriptionId { get; init; }
}