using FooCommerce.MembershipAPI.Models;

namespace FooCommerce.MembershipAPI.Contracts.Responses;

public interface TokenCreationDone : ITokenUserModel
{
    string Token { get; }
}