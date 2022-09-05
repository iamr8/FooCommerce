using FooCommerce.Domain;

namespace FooCommerce.DbProvider.Entities.Identities;

public record UserSetting
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public byte Key { get; init; }  // UserSettingKey
    public string Value { get; init; }
    public Guid UserId { get; init; }
}