using FooCommerce.Domain.DbProvider;
using FooCommerce.Membership.Enums;

namespace FooCommerce.Membership.Entities
{
    public class UserInformation : Entity
    {
        public UserInformationType Type { get; set; }
        public string Value { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}