namespace FooCommerce.IdentityAPI.Contracts;

public interface AuthTokenStateUpdateFailed
    : IAuthTokenId
{
    string Message { get; }
}