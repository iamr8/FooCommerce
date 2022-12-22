using System.Security.Claims;

namespace FooCommerce.Services.MembershipAPI.Contracts;

public interface UserClaimFound
{
    ClaimsPrincipal ClaimsPrincipal { get; }
}