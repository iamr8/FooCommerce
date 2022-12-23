using FooCommerce.MembershipService.Enums;

namespace FooCommerce.MembershipService.Models;

public record UserInformationModel
{
    public UserInformationType Type { get; init; }
    public string Value { get; init; }
}