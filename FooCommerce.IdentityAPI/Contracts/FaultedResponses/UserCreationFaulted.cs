using FooCommerce.IdentityAPI.Contracts.FaultedResponses.Enums;

namespace FooCommerce.IdentityAPI.Contracts.FaultedResponses;

public interface UserCreationFaulted
{
    UserCreationFault Fault { get; }
}