namespace FooCommerce.IdentityAPI.Worker.Contracts;

public interface UserCreated
{
    Guid CommunicationId { get; }
}