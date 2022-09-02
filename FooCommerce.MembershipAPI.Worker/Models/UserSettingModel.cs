using FooCommerce.MembershipAPI.Worker.Enums;

namespace FooCommerce.MembershipAPI.Worker.Models;

public record UserSettingModel
{
    public UserSettingKey Key { get; init; }
    public string Value { get; init; }
}