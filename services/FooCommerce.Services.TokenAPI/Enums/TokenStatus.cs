namespace FooCommerce.TokenService.Enums;

public enum TokenStatus
{
    Validated,
    Expired,
    CodeInvalid,
    MaxRetryExceeded,
    NotFound
}