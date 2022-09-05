using FooCommerce.IdentityAPI.Contracts.FaultedResponses.Enums;
using FooCommerce.IdentityAPI.Dtos;

namespace FooCommerce.IdentityAPI.Contracts.FaultedResponses;

public interface UserClaimFindingFaulted
{
    UserClaimFindingFault Fault { get; }
    UserCommunicationModel Communication { get; }
}