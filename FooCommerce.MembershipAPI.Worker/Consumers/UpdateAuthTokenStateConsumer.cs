using System.Text.Json;

using Dapper;

using FooCommerce.Application.DbProvider;
using FooCommerce.Core.DbProvider;
using FooCommerce.MembershipAPI.Contracts;
using FooCommerce.MembershipAPI.Worker.DbProvider.Entities;

using MassTransit;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.MembershipAPI.Worker.Consumers;

public class UpdateAuthTokenStateConsumer
    : IConsumer<UpdateAuthTokenState>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly ILogger<UpdateAuthTokenStateConsumer> _logger;

    public UpdateAuthTokenStateConsumer(IDbConnectionFactory dbConnectionFactory,
        ILogger<UpdateAuthTokenStateConsumer> logger, IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
        _dbContextFactory = dbContextFactory;
    }

    public async Task Consume(ConsumeContext<UpdateAuthTokenState> context)
    {
        using var dbConnection = _dbConnectionFactory.CreateConnection();
        const string query = $"SELECT TOP(1) [token].{nameof(AuthToken.More)} " +
                             $"FROM [AuthTokens] AS [token] " +
                             $"WHERE [token].{nameof(AuthToken.Id)} = @AuthTokenId";
        var jsonMore = await dbConnection.QuerySingleAsync<string>(query, new
        {
            AuthTokenId = context.Message.AuthTokenId
        });
        if (string.IsNullOrEmpty(jsonMore))
        {
            _logger.LogError("Unable to save User Notification for Auth Token {0} as Sent.", context.Message.AuthTokenId);
            await context.RespondAsync<UpdateAuthTokenStateFailed>(new
            {
                AuthTokenId = context.Message.AuthTokenId
            });
            return;
        }

        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(context.CancellationToken);
        var transaction = await dbContext.Database.BeginTransactionAsync(context.CancellationToken);
        try
        {
            // Must be updated through IDbConnectionFactory
            var token = dbContext.Attach(new AuthToken { Id = context.Message.AuthTokenId }).Entity;
            token.Authorized = DateTimeOffset.UtcNow;
            dbContext.Set<AuthToken>().Update(token);
            await dbContext.SaveChangesAsync(context.CancellationToken);

            await transaction.CommitAsync(context.CancellationToken);
            var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonMore);
            await context.RespondAsync<UpdateAuthTokenStateSuccess>(new
            {
                AuthTokenId = context.Message.AuthTokenId,
                Data = (IReadOnlyDictionary<string, object>)dict!
            });
            return;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(context.CancellationToken);
            await context.RespondAsync<UpdateAuthTokenStateFailed>(new
            {
                AuthTokenId = context.Message.AuthTokenId
            });
        }
    }
}