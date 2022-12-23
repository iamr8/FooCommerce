using System.Security.Claims;

namespace FooCommerce.MembershipService.Contracts;

public interface UserClaimFound
{
    ClaimsPrincipal ClaimsPrincipal { get; }
}