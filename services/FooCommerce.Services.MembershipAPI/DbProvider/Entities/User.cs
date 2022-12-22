using FooCommerce.Domain;

namespace FooCommerce.Services.MembershipAPI.DbProvider.Entities;

public record User
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public Guid? ParentId { get; init; }
    public virtual User Parent { get; init; }
    public virtual ICollection<UserCommunication> Communications { get; init; }
    public virtual ICollection<UserInformation> Information { get; init; }
    public virtual ICollection<UserLockout> Lockouts { get; init; }
    public virtual ICollection<UserPassword> Passwords { get; init; }
    public virtual ICollection<UserRole> Roles { get; init; }
    public virtual ICollection<UserSetting> Settings { get; init; }
    public virtual ICollection<User> Children { get; init; }
}