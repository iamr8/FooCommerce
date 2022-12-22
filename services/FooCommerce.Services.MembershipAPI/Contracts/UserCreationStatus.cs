using FooCommerce.Services.MembershipAPI.Enums;

namespace FooCommerce.Services.MembershipAPI.Contracts;

public interface UserCreationStatus
{
    Guid? CommunicationId { get; }
    UserCreationFault? Fault { get; }
    string ExceptionMessage { get; }
}