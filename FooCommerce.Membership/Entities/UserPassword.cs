using FooCommerce.Application.DbProvider;

namespace FooCommerce.Membership.Entities
{
    public record UserPassword : Entity
    {
        public string Salt { get; set; }
        public string Hash { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}