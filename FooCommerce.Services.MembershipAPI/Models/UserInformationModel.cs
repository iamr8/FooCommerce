using FooCommerce.Services.MembershipAPI.Enums;

namespace FooCommerce.Services.MembershipAPI.Models;

public record UserInformationModel
{
    public UserInformationType Type { get; init; }
    public string Value { get; init; }
}