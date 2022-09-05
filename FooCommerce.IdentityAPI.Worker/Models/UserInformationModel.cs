using FooCommerce.IdentityAPI.Worker.Enums;

namespace FooCommerce.IdentityAPI.Worker.Models;

public record UserInformationModel
{
    public UserInformationType Type { get; init; }
    public string Value { get; init; }
}