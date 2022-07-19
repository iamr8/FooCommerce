using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Ads.Interfaces;

namespace FooCommerce.Products.DigitalProducts.Entities;

public class DigitalProductAd : Entity, IEntityProductAd<DigitalProduct>
{
    public long ExternalId { get; set; }
    public DateTime EndDate { get; set; }
    public Guid ProductId { get; set; }
    public Guid? ParentAdId { get; set; }
    public Guid UserSubscriptionId { get; set; }
    public virtual DigitalProduct Product { get; set; }
}