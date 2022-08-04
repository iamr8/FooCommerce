using FooCommerce.Application.DbProvider;
using FooCommerce.Products.Entities;
using FooCommerce.Products.Interfaces;

namespace FooCommerce.Products.PurchasableItems.SecondHandItems.Entities;

public record PurchasableSecondHandItem : Entity, IEntityProduct<PurchasableSecondHandItem>
{
    public long PublicId { get; set; }
    public Guid? CategoryId { get; set; }
    public DateTime EndDate { get; set; }
    public Guid? BaseId { get; set; }
    public Guid UserSubscriptionId { get; set; }
    public ICollection<AdFeature> Features { get; set; }
    public ICollection<AdSpecification> Specifications { get; set; }
    public ICollection<AdView> Views { get; set; }
    public ICollection<AdImage> Images { get; set; }
    public ICollection<AdVideo> Videos { get; set; }
    public ICollection<AdSave> Saves { get; set; }
    public ICollection<AdLike> Likes { get; set; }
    public ICollection<AdComment> Comments { get; set; }
    public ICollection<AdLocation> Locations { get; set; }
    public PurchasableSecondHandItem? Base { get; set; }
    public ICollection<PurchasableSecondHandItem> Extensions { get; set; }
}