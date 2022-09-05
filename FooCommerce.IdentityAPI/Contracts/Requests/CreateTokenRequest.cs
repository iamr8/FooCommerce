namespace FooCommerce.IdentityAPI.Contracts.Requests;

public interface CreateTokenRequest
{
    Guid UserCommunicationId { get; }
}