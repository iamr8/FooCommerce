using System.ComponentModel;

using FooCommerce.IdentityAPI.Worker.Enums;
using FooCommerce.IdentityAPI.Worker.Exceptions;

namespace FooCommerce.IdentityAPI.Worker.Services;

public interface ITokenStore
{
    /// <summary>
    /// Creates a unique 6-digit token for the given parameters.
    /// </summary>
    /// <param name="tokenId"></param>
    /// <param name="communicationId"></param>
    /// <param name="purpose"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="TokenAdditionErrorException"></exception>
    /// <returns>A <see cref="string"/> value, which is generated according to the given data.</returns>
    string Generate(Guid tokenId, Guid communicationId, TokenRequestPurpose purpose, TimeSpan timeout);

    /// <summary>
    /// Authorizes a token with the corresponding unique id.
    /// </summary>
    /// <param name="tokenId"></param>
    /// <param name="token"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="TokenNotFoundException"></exception>
    /// <exception cref="TokenMismatchException"></exception>
    /// <exception cref="TokenInvalidationErrorException"></exception>
    void Validate(Guid tokenId, string token);
}