using System.Security.Claims;

namespace FooCommerce.IdentityAPI.Worker.Contracts;

public interface UserClaimFound
{
    ClaimsPrincipal ClaimsPrincipal { get; }
}