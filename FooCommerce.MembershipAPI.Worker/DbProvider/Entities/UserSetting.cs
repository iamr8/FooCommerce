using FooCommerce.Domain;
using FooCommerce.MembershipAPI.Worker.Enums;

namespace FooCommerce.MembershipAPI.Worker.DbProvider.Entities;

public record UserSetting
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public UserSettingKey Key { get; init; }
    public string Value { get; init; }
    public Guid UserId { get; init; }
}