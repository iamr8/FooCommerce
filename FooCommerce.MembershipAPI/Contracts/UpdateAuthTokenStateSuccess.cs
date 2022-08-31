namespace FooCommerce.MembershipAPI.Contracts;

public interface UpdateAuthTokenStateSuccess
    : IAuthTokenId
{
    public IReadOnlyDictionary<string, object> Data { get; }
}