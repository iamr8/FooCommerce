using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Domain.Entities;
using FooCommerce.Products.Domain.Interfaces;

namespace FooCommerce.Products.Events.Entities;

public record Event : Entity, IEntityProduct<Event>, IEntityProductLocation
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
    public Event? Base { get; set; }
    public ICollection<Event> Extensions { get; set; }
}