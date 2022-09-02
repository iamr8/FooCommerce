using FooCommerce.MembershipAPI.Contracts.FaultedResponses.Enums;
using FooCommerce.MembershipAPI.Enums;

namespace FooCommerce.MembershipAPI.Contracts.FaultedResponses;

public interface UserCreationFaulted
{
    UserCreationFault Fault { get; }
}