using System.Security.Claims;

using FooCommerce.IdentityAPI.Worker.Exceptions;
using FooCommerce.IdentityAPI.Worker.Models;

namespace FooCommerce.IdentityAPI.Worker.Helpers;

public static class ClaimedUserHelper
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="principal"></param>
    /// <exception cref="NullReferenceException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="UserNotAuthenticatedException"></exception>
    /// <returns></returns>
    public static IClaimedUser GetClaimedUser(this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        if (!principal.Identity.IsAuthenticated)
            throw new UserNotAuthenticatedException();

        return new ClaimedUser(principal);
    }
}