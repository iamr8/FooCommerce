using FooCommerce.Services.MembershipAPI.Dtos;
using FooCommerce.Services.MembershipAPI.Enums;

namespace FooCommerce.Services.MembershipAPI.Contracts;

public interface UserClaimFindingFaulted
{
    UserClaimFindingFault Fault { get; }
    UserCommunicationModel Communication { get; }
}