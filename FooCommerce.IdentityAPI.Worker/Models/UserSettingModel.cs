using FooCommerce.IdentityAPI.Worker.Enums;

namespace FooCommerce.IdentityAPI.Worker.Models;

public record UserSettingModel
{
    public UserSettingKey Key { get; init; }
    public string Value { get; init; }
}