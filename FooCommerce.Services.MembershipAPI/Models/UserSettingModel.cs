using FooCommerce.Services.MembershipAPI.Enums;

namespace FooCommerce.Services.MembershipAPI.Models;

public record UserSettingModel
{
    public UserSettingKey Key { get; init; }
    public string Value { get; init; }
}