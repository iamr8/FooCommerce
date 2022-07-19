using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Ads.Interfaces;

namespace FooCommerce.Products.PurchasableItems.Entities;

public class PurchasableItemAd : Entity, IEntityProductAd<PurchasableItem>
{
    public long ExternalId { get; set; }
    public DateTime EndDate { get; set; }
    public Guid ProductId { get; set; }
    public Guid? ParentAdId { get; set; }
    public Guid UserSubscriptionId { get; set; }
    public PurchasableItem Product { get; set; }
}