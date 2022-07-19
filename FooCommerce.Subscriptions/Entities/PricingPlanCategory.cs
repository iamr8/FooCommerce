using FooCommerce.Domain.DbProvider;

namespace FooCommerce.Subscriptions.Entities
{
    public class PricingPlanCategory : Entity
    {
        public virtual ICollection<PricingPlan> Plans { get; set; }
    }
}