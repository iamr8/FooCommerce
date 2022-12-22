using FooCommerce.IdentityAPI.Worker.Contracts.Enums;
using FooCommerce.IdentityAPI.Worker.Dtos;
using FooCommerce.IdentityAPI.Worker.Enums;

namespace FooCommerce.IdentityAPI.Worker.Contracts;

public interface UserClaimFindingFaulted
{
    UserClaimFindingFault Fault { get; }
    UserCommunicationModel Communication { get; }
}