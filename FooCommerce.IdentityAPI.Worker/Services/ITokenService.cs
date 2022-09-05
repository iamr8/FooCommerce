using FooCommerce.DbProvider.Entities.Identities;
using FooCommerce.IdentityAPI.Enums;

namespace FooCommerce.IdentityAPI.Worker.Services;

public interface ITokenService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="communicationId"></param>
    /// <param name="token"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    Task<AuthToken> CreateAuthTokenAsync(Guid communicationId, string token, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<string, object>> UpdateAuthTokenStateAsync(Guid authTokenId, AuthTokenState state, CancellationToken cancellationToken = default);
}