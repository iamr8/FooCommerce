using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Domain.Interfaces;

namespace FooCommerce.Products.PurchasableItems.SecondHandItems.Entities;

public class PurchasableSecondHandItemAd : Entity, IEntityProductAd<PurchasableSecondHandItem>
{
    public long ExternalId { get; set; }
    public DateTime EndDate { get; set; }
    public Guid ProductId { get; set; }
    public Guid? ParentAdId { get; set; }
    public Guid UserSubscriptionId { get; set; }
    public PurchasableSecondHandItem Product { get; set; }
}