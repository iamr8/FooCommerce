using FooCommerce.Domain.DbProvider;

namespace FooCommerce.Membership.Entities
{
    public class UserLockout : Entity
    {
        public DateTime? EndTime { get; set; } // When null, means permanently
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}