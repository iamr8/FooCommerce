namespace FooCommerce.MembershipAPI.Contracts;

public interface AuthTokenStateUpdateSuccess
    : IAuthTokenId
{
    public IReadOnlyDictionary<string, object> Data { get; }
}