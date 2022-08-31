using FooCommerce.MembershipAPI.Enums;

namespace FooCommerce.MembershipAPI.Contracts;

public interface UpdateAuthTokenState
    : IAuthTokenId
{
    AuthTokenState State { get; }
}