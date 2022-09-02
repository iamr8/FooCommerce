namespace FooCommerce.MembershipAPI.Contracts;

public interface AuthTokenStateUpdateFailed
    : IAuthTokenId
{
    string Message { get; }
}