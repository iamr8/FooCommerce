using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Ads.Interfaces;

namespace FooCommerce.Products.PurchasableItems.SecondHandItems.Entities;

public class PurchasableSecondHandItemAd : Entity, IAd<PurchasableSecondHandItem>
{
    public DateTime EndDate { get; set; }
    public Guid ProductId { get; set; }
    public Guid? ParentAdId { get; set; }
    public Guid UserSubscriptionId { get; set; }
    public PurchasableSecondHandItem Product { get; set; }
}