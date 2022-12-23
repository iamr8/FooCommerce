namespace FooCommerce.Services.TokenAPI.Enums;

public enum TokenStatus
{
    Validated,
    Expired,
    CodeInvalid,
    MaxRetryExceeded,
    NotFound
}