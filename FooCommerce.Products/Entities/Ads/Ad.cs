using FooCommerce.Domain.DbProvider;

namespace FooCommerce.Products.Entities.Ads
{
    public class Ad : Entity
    {
        public Guid? AdId { get; set; }
        public Guid UserSubscriptionId { get; set; }
    }
}