using FooCommerce.Domain.DbProvider;

namespace FooCommerce.Subscriptions.Entities
{
    public class UserSubscription : Entity
    {
        public Guid UserId { get; set; }
        public Guid PlanId { get; set; }
    }
}