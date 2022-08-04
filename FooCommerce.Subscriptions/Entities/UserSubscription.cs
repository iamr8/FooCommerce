using FooCommerce.Application.DbProvider;

namespace FooCommerce.Subscriptions.Entities
{
    public record UserSubscription : Entity
    {
        public Guid UserId { get; set; }
        public Guid PlanId { get; set; }
        public virtual PricingPlan Plan { get; set; }
    }
}