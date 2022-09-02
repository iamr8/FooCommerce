using FooCommerce.Domain.Enums;
using FooCommerce.MembershipAPI.Contracts;
using FooCommerce.MembershipAPI.Enums;
using FooCommerce.MembershipAPI.Exceptions;
using FooCommerce.MembershipAPI.Worker.DbProvider;
using FooCommerce.MembershipAPI.Worker.DbProvider.Entities;
using FooCommerce.MembershipAPI.Worker.Enums;

using MassTransit;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.MembershipAPI.Worker.Services.Repositories;

public class TokenService : ITokenService
{
    private readonly IRequestClient<UpdateAuthTokenState> _updateAuthTokenState;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public TokenService(IDbContextFactory<AppDbContext> dbContextFactory, IRequestClient<UpdateAuthTokenState> updateAuthTokenState)
    {
        _updateAuthTokenState = updateAuthTokenState;
        _dbContextFactory = dbContextFactory;
    }

    public static string GenerateToken(CommunicationType type)
    {
        switch (type)
        {
            case CommunicationType.Email_Message:
                return Guid.NewGuid().ToString("N");

            case CommunicationType.Mobile_Sms:
                var instance = new Random();
                var num = instance.NextInt64(100_000, 999_999);
                return num.ToString();

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public async Task<AuthToken> CreateAuthTokenAsync(Guid communicationId, string token, CancellationToken cancellationToken = default)
    {
        if (communicationId == Guid.Empty) throw new ArgumentNullException(nameof(token));
        if (token == null) throw new ArgumentNullException(nameof(token));

        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var authToken = dbContext.Set<AuthToken>().Add(new AuthToken
            {
                Action = (byte)AuthTokenAction.Request_EmailVerification,
                UserCommunicationId = communicationId,
                Token = token,
            }).Entity;
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return authToken;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            return null;
        }
    }

    /// <summary>
    /// Update state of an <see cref="AuthToken"/> row with the given <see cref="Guid"/> id to the given <see cref="AuthTokenState"/>.
    /// </summary>
    /// <param name="authTokenId"></param>
    /// <param name="state"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AuthTokenStateUpdateException"></exception>
    /// <returns></returns>
    public async Task<IReadOnlyDictionary<string, object>> UpdateAuthTokenStateAsync(Guid authTokenId, AuthTokenState state, CancellationToken cancellationToken = default)
    {
        if (authTokenId == Guid.Empty) throw new ArgumentNullException(nameof(authTokenId));
        var (success, failed) =
            await _updateAuthTokenState.GetResponse<AuthTokenStateUpdateSuccess, AuthTokenStateUpdateFailed>(new
            {
                AuthTokenId = authTokenId,
                State = state
            }, cancellationToken);

        if (failed.IsCompleted)
        {
            var faultedData = await failed;
            throw new AuthTokenStateUpdateException(faultedData.Message.Message, faultedData);
        }

        var succeededData = await success;
        var dict = succeededData.Message.Data;
        return dict;
    }
}