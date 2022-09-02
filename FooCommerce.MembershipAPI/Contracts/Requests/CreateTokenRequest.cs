namespace FooCommerce.MembershipAPI.Contracts.Requests;

public interface CreateTokenRequest
{
    Guid UserCommunicationId { get; }
}