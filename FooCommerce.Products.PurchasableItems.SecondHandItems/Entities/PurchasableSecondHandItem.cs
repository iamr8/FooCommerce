using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Domain.Ads.Entities;
using FooCommerce.Products.Domain.Interfaces;

namespace FooCommerce.Products.PurchasableItems.SecondHandItems.Entities;

public class PurchasableSecondHandItem : Entity, IEntityProduct<PurchasableSecondHandItem>
{
    public long ExternalId { get; set; }
    public Guid? CategoryId { get; set; }
    public DateTime EndDate { get; set; }
    public Guid? BaseId { get; set; }
    public Guid UserSubscriptionId { get; set; }
    public ICollection<AdFeature> Features { get; set; }
    public ICollection<AdSpecification> Specifications { get; set; }
    public ICollection<AdView> Views { get; set; }
    public ICollection<AdImage> Images { get; set; }
    public ICollection<AdWish> Wishes { get; set; }
    public ICollection<AdLocation> Locations { get; set; }
    public PurchasableSecondHandItem? Base { get; set; }
    public ICollection<PurchasableSecondHandItem> Extensions { get; set; }
}