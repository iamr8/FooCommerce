using FooCommerce.MembershipAPI.Worker.Enums;

namespace FooCommerce.MembershipAPI.Worker.Models;

public record UserInformationModel
{
    public UserInformationType Type { get; init; }
    public string Value { get; init; }
}