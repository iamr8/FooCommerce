using System.ComponentModel;
using System.Data;

using Dapper;

using FooCommerce.DbProvider;
using FooCommerce.DbProvider.Entities.Identities;
using FooCommerce.Domain.Enums;
using FooCommerce.IdentityAPI.Worker.Dtos;

namespace FooCommerce.IdentityAPI.Worker.Services.Repositories;

public class CommunicationsManager : ICommunicationsManagerService
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public CommunicationsManager(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    #region Statics

    ///// <summary>
    /////
    ///// </summary>
    ///// <param name="communicationModel"></param>
    ///// <param name="dbConnection"></param>
    ///// <returns></returns>
    ///// <exception cref="ArgumentNullException"></exception>
    //internal static async Task<TokenUserModel> GetTokenUserModelAsync(UserCommunicationModel communicationModel, IDbConnection dbConnection)
    //{
    //    if (communicationModel == null) throw new ArgumentNullException(nameof(communicationModel));
    //    if (dbConnection == null) throw new ArgumentNullException(nameof(dbConnection));

    //    var sql = $"SELECT TOP(1) [communication].{nameof(UserCommunication.UserId)}, [userInformation].{nameof(UserInformation.Value)} AS [Name]" +
    //              "FROM [UserCommunications] AS [communication] " +
    //              $"CROSS APPLY (SELECT TOP(1) [userInformation].{nameof(UserInformation.Value)} FROM [UserInformation] AS [userInformation] WHERE [userInformation].{nameof(UserInformation.UserId)} = [communication].{nameof(UserCommunication.UserId)} AND [userInformation].{nameof(UserInformation.Type)} = {(byte)UserInformationType.Name} ORDER BY [userInformation].{nameof(UserInformation.Type)}, [userInformation].{nameof(UserInformation.Created)} DESC) AS [userInformation] " +
    //              $"WHERE [communication].{nameof(UserCommunication.Id)} = @UserCommunicationId";
    //    var data = await dbConnection.QuerySingleOrDefaultAsync(sql, new { UserCommunicationId = communicationModel.Id });
    //    if (data == null || data.Value == Guid.Empty)
    //        return null;

    //    var userId = (Guid)data.UserId;
    //    var name = (string)data.Name;

    //    var model = new TokenUserModel
    //    {
    //        UserId = userId,
    //        Name = name,
    //        Communication = communicationModel,
    //    };

    //    return model;
    //}

    /// <summary>
    ///
    /// </summary>
    /// <param name="userCommunicationId"></param>
    /// <param name="dbConnection"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<UserCommunicationModel> GetModelByIdAsync(Guid userCommunicationId, IDbConnection dbConnection)
    {
        if (dbConnection == null) throw new ArgumentNullException(nameof(dbConnection));

        const string sql = $"SELECT TOP(1) [communication].{nameof(UserCommunication.Type)}, [communication].{nameof(UserCommunication.Value)}, [communication].{nameof(UserCommunication.IsVerified)} " +
                            "FROM [UserCommunications] AS [communication] " +
                           $"WHERE [communication].{nameof(UserCommunication.Id)} = @UserCommunicationId";
        var data = await dbConnection.QuerySingleOrDefaultAsync(sql, new { UserCommunicationId = userCommunicationId });

        var communication = new UserCommunicationModel(
            userCommunicationId,
            (CommType)(byte)data.Type,
            (string)data.Value,
            (bool)data.IsVerified);

        return communication;
    }

    #endregion Statics

    #region Publics

    public async Task<Guid?> GetVerifiedCommunicationIdAsync(CommType type, string value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        if (!Enum.IsDefined(typeof(CommType), type))
            throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(CommType));

        using (var dbConnection = _dbConnectionFactory.CreateConnection())
        {
            var sql = $"SELECT TOP(1) [communication].{nameof(UserCommunication.Id)} " +
                      "FROM [UserCommunications] AS [communication] " +
                      $"WHERE [communication].{nameof(UserCommunication.IsVerified)} = 1 AND [communication].{nameof(UserCommunication.Type)} = {(byte)type} AND [communication].{nameof(UserCommunication.Value)} = N'@Value'";
            var establishedId = await dbConnection.QuerySingleOrDefaultAsync<Guid?>(sql, new { Value = value.ToLowerInvariant() });
            if (establishedId == null || establishedId.Value == Guid.Empty)
                return null;

            return establishedId;
        }
    }

    //public async Task<IEnumerable<UserCommunicationModel>> GetModelsByUserIdAsync(Guid userId)
    //{
    //    if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));

    //    using (var dbConnection = _dbConnectionFactory.CreateConnection())
    //    {
    //        var sql = $"SELECT [communication].{nameof(UserCommunication.Type)}, [communication].{nameof(UserCommunication.Value)}, [communication].{nameof(UserCommunication.IsVerified)} " +
    //                  $"FROM [UserCommunications] AS [communication]" +
    //                  $"WHERE [communication].{nameof(UserCommunication.UserId)} = '@UserId'";
    //        var communications = await dbConnection.QueryAsync<UserCommunicationModel>(sql, new { UserId = userId });
    //        return communications;
    //    }
    //}

    //public Task<UserCommunicationModel> GetModelByIdAsync(Guid userCommunicationId)
    //{
    //    using (var dbConnection = _dbConnectionFactory.CreateConnection())
    //    {
    //        return GetModelByIdAsync(userCommunicationId, dbConnection);
    //    }
    //}

    //public Task<UserCommunicationModel> GetNonVerifiedModelByTypeAsync(CommunicationType type, string value)
    //{
    //    if (value == null) throw new ArgumentNullException(nameof(value));
    //    if (!Enum.IsDefined(typeof(CommunicationType), type))
    //        throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(CommunicationType));

    //    using (var dbConnection = _dbConnectionFactory.CreateConnection())
    //    {
    //        return GetNonVerifiedModelByTypeAsync(type, value, dbConnection);
    //    }
    //}

    #endregion Publics

    #region Privates

    /// <summary>
    ///
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <param name="dbConnection"></param>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>A nullable <see cref="Guid"/> value that represents Id of the non-verified <see cref="UserCommunication"/>.</returns>
    private static async Task<UserCommunicationModel> GetNonVerifiedModelByTypeAsync(CommType type, string value, IDbConnection dbConnection)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        if (dbConnection == null) throw new ArgumentNullException(nameof(dbConnection));
        if (!Enum.IsDefined(typeof(CommType), type))
            throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(CommType));

        if (dbConnection.State != ConnectionState.Open)
            throw new Exception("DbConnection must be opened already.");

        var sql = $"SELECT TOP(1) [communication].{nameof(UserCommunication.Id)} " +
                  "FROM [UserCommunications] AS [communication] " +
                  $"WHERE [communication].{nameof(UserCommunication.IsVerified)} = 0 AND [communication].{nameof(UserCommunication.Type)} = {(byte)type} AND [communication].{nameof(UserCommunication.Value)} = N'@Value'";
        var nonVerifiedId = await dbConnection.QuerySingleOrDefaultAsync<Guid?>(sql, new { Value = value.ToLowerInvariant() });
        if (nonVerifiedId == null || nonVerifiedId.Value == Guid.Empty)
            return null;

        var model = new UserCommunicationModel(nonVerifiedId.Value, type, value, false);
        return model;
    }

    #endregion Privates
}