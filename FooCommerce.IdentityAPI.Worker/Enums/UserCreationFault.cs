namespace FooCommerce.IdentityAPI.Worker.Contracts.Enums;

public enum UserCreationFault
{
    EmailAlreadyEstablished = 0,
    DatabaseTransactionFailed = 1
}