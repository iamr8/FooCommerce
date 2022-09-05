namespace FooCommerce.IdentityAPI.Contracts.FaultedResponses.Enums;

public enum TokenCreationFault
{
    CommunicationNotFound = 0,
    AlreadyEstablished = 1,
    TokenNotCreated = 2,
    TokenUserNotCreated = 3
}