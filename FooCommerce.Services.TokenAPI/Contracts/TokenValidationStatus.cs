using FooCommerce.Services.TokenAPI.Enums;

namespace FooCommerce.Services.TokenAPI.Contracts;

public interface TokenValidationStatus
{
    TokenStatus Status { get; }
}