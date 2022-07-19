using FooCommerce.Domain.DbProvider;

namespace FooCommerce.Membership.Entities
{
    public class User : Entity
    {
        public Guid? ParentId { get; set; }
        public virtual User Parent { get; set; }
        public virtual ICollection<User> NestedUsers { get; set; }
        public virtual ICollection<UserPassword> Passwords { get; set; }
        public virtual ICollection<UserInformation> Information { get; set; }
        public virtual ICollection<UserContact> Contacts { get; set; }
        public virtual ICollection<UserLockout> Lockouts { get; set; }
    }
}