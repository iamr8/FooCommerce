using FooCommerce.TokenService.Enums;

namespace FooCommerce.TokenService.Contracts;

public interface TokenFulfilled
{
    TokenStatus Status { get; }
}