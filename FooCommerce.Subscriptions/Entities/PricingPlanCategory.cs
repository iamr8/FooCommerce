using FooCommerce.Application.DbProvider;

namespace FooCommerce.Subscriptions.Entities
{
    public record PricingPlanCategory : Entity
    {
        public virtual ICollection<PricingPlan> Plans { get; set; }
    }
}