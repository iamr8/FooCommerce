namespace FooCommerce.MembershipAPI.Contracts.FaultedResponses.Enums;

public enum UserCreationFault
{
    EmailAlreadyEstablished = 0,
    DatabaseTransactionFailed = 1
}