using FooCommerce.IdentityAPI.Enums;

namespace FooCommerce.IdentityAPI.Contracts;

public interface UpdateAuthTokenState
    : IAuthTokenId
{
    AuthTokenState State { get; }
}