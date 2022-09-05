namespace FooCommerce.IdentityAPI.Contracts;

public interface AuthTokenStateUpdateSuccess
    : IAuthTokenId
{
    public IReadOnlyDictionary<string, object> Data { get; }
}