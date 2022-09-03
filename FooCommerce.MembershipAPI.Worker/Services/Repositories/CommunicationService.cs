using System.ComponentModel;
using System.Data;

using Dapper;

using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.Enums;
using FooCommerce.MembershipAPI.Dtos;
using FooCommerce.MembershipAPI.Models;
using FooCommerce.MembershipAPI.Worker.DbProvider;
using FooCommerce.MembershipAPI.Worker.DbProvider.Entities;
using FooCommerce.MembershipAPI.Worker.Enums;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.MembershipAPI.Worker.Services.Repositories;

public class CommunicationService : ICommunicationService
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IDbContextFactory<MembershipDbContext> _dbContextFactory;

    public CommunicationService(IDbConnectionFactory dbConnectionFactory, IDbContextFactory<MembershipDbContext> dbContextFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _dbContextFactory = dbContextFactory;
    }

    public static async Task<TokenUserModel> GetTokenUserModelAsync(UserCommunicationModel communicationModel, IDbConnection dbConnection, CancellationToken cancellationToken = default)
    {
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var data = await dbConnection.QuerySingleOrDefaultAsync(
                $"SELECT TOP(1) [communication].{nameof(UserCommunication.UserId)}, [userInformation].{nameof(UserInformation.Value)} AS [Name]" +
                "FROM [UserCommunications] AS [communication] " +
                $"CROSS APPLY (SELECT TOP(1) [userInformation].{nameof(UserInformation.Value)} FROM [UserInformation] AS [userInformation] WHERE [userInformation].{nameof(UserInformation.UserId)} = [communication].{nameof(UserCommunication.UserId)} AND [userInformation].{nameof(UserInformation.Type)} = {(byte)UserInformationType.Name} ORDER BY [userInformation].{nameof(UserInformation.Type)}, [userInformation].{nameof(UserInformation.Created)} DESC) AS [userInformation] " +
                $"WHERE [communication].{nameof(UserCommunication.Id)} = N'@UserCommunicationId'",
                new { UserCommunicationId = communicationModel.Id });
            if (data == null || data.Value == Guid.Empty)
                return null;

            var userId = (Guid)data.UserId;
            var name = (string)data.Name;

            var model = new TokenUserModel
            {
                UserId = userId,
                Name = name,
                Communication = communicationModel,
            };

            return model;
        }
    }

    public static async Task<Guid?> GetVerifiedCommunicationIdAsync(CommunicationType type, string value, IDbConnection dbConnection, CancellationToken cancellationToken = default)
    {
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var establishedId = await dbConnection.QuerySingleOrDefaultAsync<Guid?>(
                $"SELECT TOP(1) [communication].{nameof(UserCommunication.Id)} " +
                "FROM [UserCommunications] AS [communication] " +
                $"WHERE [communication].{nameof(UserCommunication.IsVerified)} = 1 AND [communication].{nameof(UserCommunication.Type)} = {(byte)type} AND [communication].{nameof(UserCommunication.Value)} = N'@Value'",
                new { Value = value.ToLowerInvariant() });
            if (establishedId == null || establishedId.Value == Guid.Empty)
                return null;

            return establishedId;
        }
    }

    public static async Task<UserCommunicationModel> GetModelByIdAsync(Guid userCommunicationId, IDbConnection dbConnection, CancellationToken cancellationToken = default)
    {
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var communication = await dbConnection.QuerySingleOrDefaultAsync<UserCommunicationModel>(
                $"SELECT TOP(1) [communication].{nameof(UserCommunication.Type)}, [communication].{nameof(UserCommunication.Value)}, [communication].{nameof(UserCommunication.IsVerified)} " +
                $"FROM [UserCommunications] AS [communication]" +
                $"WHERE [communication].{nameof(UserCommunication.Id)} = '@UserCommunicationId'",
                new
                {
                    UserCommunicationId = userCommunicationId
                });

            return communication;
        }
    }

    public Task<UserCommunicationModel> GetNonVerifiedModelByTypeAsync(CommunicationType type, string value, CancellationToken cancellationToken = default)
    {
        using var dbConnection = _dbConnectionFactory.CreateConnection();

        return GetNonVerifiedModelByTypeAsync(type, value, dbConnection, cancellationToken);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <param name="dbConnection"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>A nullable <see cref="Guid"/> value that represents Id of the non-verified <see cref="UserCommunication"/>.</returns>
    private static async Task<UserCommunicationModel> GetNonVerifiedModelByTypeAsync(CommunicationType type, string value, IDbConnection dbConnection, CancellationToken cancellationToken = default)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        if (dbConnection == null) throw new ArgumentNullException(nameof(dbConnection));
        if (!Enum.IsDefined(typeof(CommunicationType), type))
            throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(CommunicationType));
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var nonVerifiedId = await dbConnection.QuerySingleOrDefaultAsync<Guid?>(
                $"SELECT TOP(1) [communication].{nameof(UserCommunication.Id)} " +
                "FROM [UserCommunications] AS [communication] " +
                $"WHERE [communication].{nameof(UserCommunication.IsVerified)} = 0 AND [communication].{nameof(UserCommunication.Type)} = {(byte)type} AND [communication].{nameof(UserCommunication.Value)} = N'@Value'",
                new { Value = value.ToLowerInvariant() });
            if (nonVerifiedId == null || nonVerifiedId.Value == Guid.Empty)
                return null;

            var model = new UserCommunicationModel(nonVerifiedId.Value, type, value, false);
            return model;
        }
    }

    public async Task<IEnumerable<UserCommunicationModel>> GetModelsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
        using var dbConnection = _dbConnectionFactory.CreateConnection();
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var communications = await dbConnection.QueryAsync<UserCommunicationModel>(
                $"SELECT [communication].{nameof(UserCommunication.Type)}, [communication].{nameof(UserCommunication.Value)}, [communication].{nameof(UserCommunication.IsVerified)} " +
                $"FROM [UserCommunications] AS [communication]" +
                $"WHERE [communication].{nameof(UserCommunication.UserId)} = '@UserId'",
                new
                {
                    UserId = userId
                });
            if (communications == null || !communications.Any())
                return null;

            var result = communications.AsList();
            return result;
        }
    }

    public Task<UserCommunicationModel> GetModelByIdAsync(Guid userCommunicationId, CancellationToken cancellationToken = default)
    {
        using var dbConnection = _dbConnectionFactory.CreateConnection();
        return GetModelByIdAsync(userCommunicationId, dbConnection, cancellationToken);
    }
}