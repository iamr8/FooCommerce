using FooCommerce.MembershipAPI.Contracts.FaultedResponses.Enums;

namespace FooCommerce.MembershipAPI.Contracts.FaultedResponses;

public interface TokenCreationFaulted
{
    TokenCreationFault Fault { get; }
}