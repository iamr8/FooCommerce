using FooCommerce.MembershipService.Enums;

namespace FooCommerce.MembershipService.Models;

public record UserSettingModel
{
    public UserSettingKey Key { get; init; }
    public string Value { get; init; }
}