using FooCommerce.Domain.DbProvider.Interfaces;
using FooCommerce.Products.Domain.Ads.Entities;

namespace FooCommerce.Products.Domain.Interfaces
{
    public interface IEntityProduct : IEntityExternalId
    {
        Guid? CategoryId { get; set; }

        DateTime EndDate { get; set; }
        Guid? BaseId { get; set; }
        Guid UserSubscriptionId { get; set; }
        ICollection<AdFeature> Features { get; set; }
        ICollection<AdSpecification> Specifications { get; set; }
        ICollection<AdView> Views { get; set; }
        ICollection<AdImage> Images { get; set; }
        ICollection<AdWish> Wishes { get; set; }
        ICollection<AdLocation> Locations { get; set; }
    }
}