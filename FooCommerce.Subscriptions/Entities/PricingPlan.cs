using FooCommerce.Domain.DbProvider;

namespace FooCommerce.Subscriptions.Entities
{
    public class PricingPlan : Entity
    {
        public int? MaxPictures { get; set; }
        public int? MinPictures { get; set; }
        public int? NumberOfAds { get; set; }
        public int? Days { get; set; }
        public Guid CategoryId { get; set; }
        public virtual PricingPlanCategory Category { get; set; }
        public virtual ICollection<UserSubscription> UserSubscriptions { get; set; }
    }
}