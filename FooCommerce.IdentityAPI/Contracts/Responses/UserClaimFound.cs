using System.Security.Claims;

namespace FooCommerce.IdentityAPI.Contracts.Responses;

public interface UserClaimFound
{
    ClaimsPrincipal ClaimsPrincipal { get; }
}