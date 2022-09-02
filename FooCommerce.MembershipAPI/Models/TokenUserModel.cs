using FooCommerce.MembershipAPI.Dtos;

namespace FooCommerce.MembershipAPI.Models;

public record TokenUserModel : ITokenUserModel
{
    public Guid UserId { get; init; }
    public string Name { get; init; }
    public UserCommunicationModel Communication { get; init; }
}