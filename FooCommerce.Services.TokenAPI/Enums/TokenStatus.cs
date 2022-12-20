namespace FooCommerce.Services.TokenAPI.Enums;

public enum TokenStatus
{
    Validated,
    Expired,
    TokenInvalid,
    MaxRetryExceeded,
    NotFound
}