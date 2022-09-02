using FooCommerce.MembershipAPI.Contracts.FaultedResponses.Enums;
using FooCommerce.MembershipAPI.Dtos;

namespace FooCommerce.MembershipAPI.Contracts.FaultedResponses;

public interface UserClaimFindingFaulted
{
    UserClaimFindingFault Fault { get; }
    UserCommunicationModel Communication { get; }
}