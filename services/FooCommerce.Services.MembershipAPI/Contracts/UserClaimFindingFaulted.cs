using FooCommerce.MembershipService.Dtos;
using FooCommerce.MembershipService.Enums;

namespace FooCommerce.MembershipService.Contracts;

public interface UserClaimFindingFaulted
{
    UserClaimFindingFault Fault { get; }
    UserCommunicationModel Communication { get; }
}