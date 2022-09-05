using FooCommerce.IdentityAPI.Models;

namespace FooCommerce.IdentityAPI.Contracts.Responses;

public interface TokenCreationDone : ITokenUserModel
{
    string Token { get; }
}