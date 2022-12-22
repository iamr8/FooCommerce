using System.Collections.Concurrent;
using System.ComponentModel;
using System.Security.Cryptography;

using FooCommerce.IdentityAPI.Worker.Enums;
using FooCommerce.IdentityAPI.Worker.Exceptions;
using FooCommerce.IdentityAPI.Worker.Models;

namespace FooCommerce.IdentityAPI.Worker.Services.Repositories;

public class TokenStore : ITokenStore
{
    private readonly ConcurrentDictionary<Guid, TokenModel> _tokens = new();

    private static SemaphoreSlim _semaphore = new(1, 1);

    #region Publics

    public void Validate(Guid tokenId, string token)
    {
        if (tokenId == Guid.Empty) throw new ArgumentNullException(nameof(tokenId));
        if (token == null) throw new ArgumentNullException(nameof(token));

        var hasToken = _tokens.TryGetValue(tokenId, out var model);
        if (!hasToken)
            throw new TokenNotFoundException();

        if (model.Code != token)
            throw new TokenMismatchException();

        Invalidate(tokenId);
    }

    public string Get(Guid tokenId)
    {
        try
        {
            _semaphore.Wait();

            return _tokens[tokenId].Code;
        }
        catch (Exception)
        {
            return null;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public string Generate(Guid tokenId, Guid communicationId, TokenRequestPurpose purpose, TimeSpan timeout)
    {
        if (communicationId == Guid.Empty)
            throw new ArgumentNullException(nameof(communicationId));
        if (!Enum.IsDefined(typeof(TokenRequestPurpose), purpose))
            throw new InvalidEnumArgumentException(nameof(purpose), (int)purpose, typeof(TokenRequestPurpose));

        try
        {
            _semaphore.Wait();

            var duplicate = _tokens.FirstOrDefault(x => x.Value.Purpose == purpose && x.Value.UserCommunicationId == communicationId);
            if (duplicate.Value != null)
                return duplicate.Value.Code;

            var code = RandomNumberGenerator.GetInt32(100_000, 999_999).ToString();
            var token = new TokenModel
            {
                Purpose = purpose,
                Code = code,
                UserCommunicationId = communicationId,
                Timer = new Timer(_tokenId =>
                {
                    try
                    {
                        Invalidate((Guid)_tokenId);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }, tokenId, (long)timeout.TotalMilliseconds, -1),
                Timeout = timeout
            };
            var added = _tokens.TryAdd(tokenId, token);
            if (!added)
                throw new TokenAdditionErrorException();

            return token.Code;
        }
        catch (Exception e)
        {
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    #endregion Publics

    #region Privates

    /// <summary>
    /// Invalidates ax existing token.
    /// </summary>
    /// <param name="tokenId"></param>
    /// <exception cref="TokenInvalidationErrorException"></exception>
    private void Invalidate(Guid tokenId)
    {
        var exists = _tokens.TryGetValue(tokenId, out var token);
        if (!exists)
            throw new TokenInvalidationErrorException();

        token.Timer.Dispose();

        var removed = _tokens.TryRemove(tokenId, out _);
        if (!removed)
            throw new TokenInvalidationErrorException();
    }

    #endregion Privates
}