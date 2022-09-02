using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace FooCommerce.MembershipAPI.Contracts.Responses;

public interface UserClaimFound
{
    ClaimsPrincipal ClaimsPrincipal { get; }
    AuthenticationProperties Properties { get; }
}