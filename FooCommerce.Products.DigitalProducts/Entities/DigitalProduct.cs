using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Entities;
using FooCommerce.Products.Interfaces;

namespace FooCommerce.Products.DigitalProducts.Entities;

public record DigitalProduct : Entity, IEntityProduct<DigitalProduct>
{
    public string Name { get; set; }
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
    public DigitalProduct? Base { get; set; }
    public ICollection<DigitalProduct> Extensions { get; set; }
}