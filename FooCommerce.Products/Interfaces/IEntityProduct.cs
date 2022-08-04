using FooCommerce.Application.DbProvider.Interfaces;
using FooCommerce.Products.Entities;

namespace FooCommerce.Products.Interfaces
{
    public interface IEntityProduct : IEntityPublicId
    {
        Guid? CategoryId { get; set; }

        DateTime EndDate { get; set; }
        Guid? BaseId { get; set; }
        Guid UserSubscriptionId { get; set; }
        ICollection<AdFeature> Features { get; set; }
        ICollection<AdSpecification> Specifications { get; set; }
        ICollection<AdView> Views { get; set; }
        ICollection<AdImage> Images { get; set; }
        ICollection<AdVideo> Videos { get; set; }
        ICollection<AdSave> Saves { get; set; }
        ICollection<AdLike> Likes { get; set; }
        ICollection<AdComment> Comments { get; set; }
    }
}