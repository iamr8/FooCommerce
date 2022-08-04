using FooCommerce.Application.DbProvider;
using FooCommerce.Membership.Enums;

namespace FooCommerce.Membership.Entities
{
    public record UserInformation : Entity
    {
        public UserInformationType Type { get; set; }
        public string Value { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}