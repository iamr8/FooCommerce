using FooCommerce.Services.TokenAPI.Enums;

namespace FooCommerce.Services.TokenAPI.Contracts;

public interface TokenFulfilled
{
    TokenStatus Status { get; }
}