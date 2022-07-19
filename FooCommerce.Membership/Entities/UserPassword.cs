using FooCommerce.Domain.DbProvider;

namespace FooCommerce.Membership.Entities
{
    public class UserPassword : Entity
    {
        public string Salt { get; set; }
        public string Hash { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}