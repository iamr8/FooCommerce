using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Entities;
using FooCommerce.Products.Interfaces;

namespace FooCommerce.Products.Foods.Entities;

public record Food : Entity, IEntityProduct<Food>
{
    public uint PublicId { get; init; }
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
    public Food? Base { get; set; }
    public ICollection<Food> Extensions { get; set; }
}