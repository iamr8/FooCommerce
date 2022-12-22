using System.Data;
using System.Globalization;
using System.Security.Claims;

using Dapper;

using FooCommerce.Caching;
using FooCommerce.Common.Protection;
using FooCommerce.DbProvider;
using FooCommerce.DbProvider.Entities.Identities;
using FooCommerce.Domain.Enums;
using FooCommerce.IdentityAPI.Worker.Contracts;
using FooCommerce.IdentityAPI.Worker.Dtos;
using FooCommerce.IdentityAPI.Worker.Enums;
using FooCommerce.IdentityAPI.Worker.Models;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.IdentityAPI.Worker.Services.Repositories;

public class UserManager : IUserManagerService
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly ICacheProvider _cacheProvider;
    private readonly ILogger<UserManager> _logger;

    public UserManager(
        IDbConnectionFactory dbConnectionFactory,
        IDbContextFactory<AppDbContext> dbContextFactory,
        ICacheProvider cacheProvider,
        ILogger<UserManager> logger
        )
    {
        _logger = logger;
        _dbContextFactory = dbContextFactory;
        _dbConnectionFactory = dbConnectionFactory;
        _cacheProvider = cacheProvider;
    }

    #region Statics

    internal static async Task<IEnumerable<UserSettingModel>> GetUserSettingsAsync(Guid userId, IDbConnection dbConnection)
    {
        const string sql = $"SELECT TOP(1) [setting].{nameof(UserSetting.Key)}, [setting].{nameof(UserSetting.Value)} " +
                           "FROM [UserSettings] AS [setting] " +
                           $"WHERE [setting].{nameof(UserSetting.UserId)} = @UserId";
        var credentialModels = await dbConnection.QueryAsync<UserSettingModel>(sql, new { UserId = userId });
        return credentialModels;
    }

    internal static async Task<IEnumerable<UserInformationModel>> GetUserInformationAsync(Guid userId, IDbConnection dbConnection)
    {
        const string sql = $"SELECT TOP(1) [information].{nameof(UserInformation.Type)}, [information].{nameof(UserInformation.Value)} " +
                           "FROM [UserInformation] AS [information] " +
                           $"WHERE [information].{nameof(UserInformation.UserId)} = @UserId";
        var credentialModels = await dbConnection.QueryAsync<UserInformationModel>(sql, new { UserId = userId });
        return credentialModels;
    }

    internal static async Task<UserCredentialModel> GetUserModelAsync(CommType type, string value, IDbConnection dbConnection)
    {
        var sql = $"SELECT TOP(1) [user].{nameof(User.Id)} AS [{nameof(UserCredentialModel.UserId)}], [password].{nameof(UserPassword.Hash)} AS [{nameof(UserCredentialModel.Hash)}], [communication].{nameof(UserCommunication.Id)} AS [{nameof(UserCredentialModel.CommunicationId)}], [role].{nameof(Role.Type)} AS [{nameof(UserCredentialModel.RoleType)}], [userRole].{nameof(UserRole.RoleId)} AS [{nameof(UserCredentialModel.RoleId)}] " +
                  "FROM [UserCommunications] AS [communication] " +
                  $"CROSS APPLY (SELECT TOP(1) [user].{nameof(User.Id)} FROM [Users] as [user] WHERE [user].{nameof(User.Id)} = [communication].{nameof(UserCommunication.UserId)}) AS [user] " +
                  $"CROSS APPLY (SELECT TOP(1) [password].{nameof(UserPassword.Hash)} FROM [UserPasswords] AS [password] WHERE [password].{nameof(UserPassword.UserId)} = [user].{nameof(User.Id)} ORDER BY [password].{nameof(UserPassword.Created)} DESC) AS [password] " +
                  $"CROSS APPLY (SELECT TOP(1) [userRole].{nameof(UserRole.RoleId)} FROM [UserRoles] AS [userRole] WHERE [userRole].{nameof(UserRole.UserId)} = [user].{nameof(User.Id)} ORDER BY [userRole].{nameof(UserRole.Created)} DESC) AS [userRole]" +
                  $"CROSS APPLY (SELECT TOP(1) [role].{nameof(Role.Type)} FROM [Roles] AS [role] WHERE [role].{nameof(Role.Id)} = [userRole].{nameof(UserRole.RoleId)}) AS [role]" +
                  $"WHERE [communication].{nameof(UserCommunication.IsVerified)} = 1 AND [communication].{nameof(UserCommunication.Type)} = {(byte)type} AND [communication].{nameof(UserCommunication.Value)} = N'@Value'";
        var credentialModel = await dbConnection.QuerySingleOrDefaultAsync<UserCredentialModel>(sql, new { Value = value.ToLowerInvariant() });
        return credentialModel;
    }

    internal static ClaimsPrincipal GetClaimsPrincipal(string username, UserCredentialModel userModel, UserCommunicationModel communicationModel, IEnumerable<UserInformationModel> informationModels, IEnumerable<UserSettingModel> settingModels)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, username),
            new(ClaimTypes.GivenName, informationModels.First(x=>x.Type == UserInformationType.Name).Value),
            new(ClaimTypes.Surname, informationModels.First(x=>x.Type == UserInformationType.Surname).Value),
            new(ClaimTypes.Country, new RegionInfo(settingModels.First(x=>x.Key == UserSettingKey.Country).Value).ToString(), typeof(RegionInfo).ToString()),
            new(ClaimTypes.Role, userModel.RoleType.ToString(),typeof(RoleType).ToString()),
            new("RoleId", userModel.RoleId.ToString("N"), typeof(Guid).ToString()),
            new(ClaimTypes.NameIdentifier, userModel.UserId.ToString("N"), typeof(Guid).ToString()),
            new(ClaimTypes.Hash, userModel.Hash)
        };

        switch (communicationModel.Type)
        {
            case CommType.Email:
                claims.Add(new Claim(ClaimTypes.Email, communicationModel.Value));
                break;

            case CommType.Sms:
                claims.Add(new Claim(ClaimTypes.MobilePhone, communicationModel.Value));
                break;

            case CommType.Push:
            default:
                throw new ArgumentOutOfRangeException(nameof(communicationModel));
        }

        var claimsIdentity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        return claimsPrincipal;
    }

    #endregion Statics

    #region Publics

    public async Task<IEnumerable<RoleModel>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        using (var dbConnection = _dbConnectionFactory.CreateConnection())
        {
            return await GetRolesAsync(dbConnection, cancellationToken);
        }
    }

    public async Task<RoleModel> GetRoleAsync(RoleType type, CancellationToken cancellationToken = default)
    {
        var roles = await GetRolesAsync(cancellationToken);
        var role = roles.Single(x => x.Type == type);
        return role;
    }

    public async Task<CreatedUserModel> CreateUserAsync(SignUpRequest model, RoleModel role, CancellationToken cancellationToken = default)
    {
        await using (var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken))
        {
            var output = new CreatedUserModel();
            var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                output.User = dbContext.Users.Add(new User()).Entity;
                await dbContext.SaveChangesAsync(cancellationToken);

                var hash = DataProtector.Hash(model.Password, 16, 32, 10_000);
                output.Password = dbContext.UserPasswords.Add(new UserPassword
                {
                    UserId = output.User.Id,
                    Hash = hash
                }).Entity;

                output.Role = dbContext.UserRoles.Add(new UserRole
                {
                    RoleId = role.Id,
                    UserId = output.User.Id
                }).Entity;

                output.Communication = dbContext.UserCommunications.Add(new UserCommunication
                {
                    Type = (byte)CommType.Email,
                    Value = model.Email.ToLowerInvariant(),
                    UserId = output.User.Id
                }).Entity;

                output.Settings ??= new List<UserSetting>();
                output.Settings.Add(dbContext.UserSettings.Add(new UserSetting
                {
                    Key = (byte)UserSettingKey.Country,
                    Value = model.Country.ToString(),
                    UserId = output.User.Id
                }).Entity);

                output.Information ??= new List<UserInformation>();
                output.Information.Add(dbContext.UserInformation.Add(new UserInformation
                {
                    Type = (byte)UserInformationType.Name,
                    Value = model.FirstName,
                    UserId = output.User.Id
                }).Entity);

                output.Information.Add(dbContext.UserInformation.Add(new UserInformation
                {
                    Type = (byte)UserInformationType.Surname,
                    Value = model.LastName,
                    UserId = output.User.Id
                }).Entity);
                await dbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
                return output;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(cancellationToken);
                return null;
            }
        }
    }

    #endregion Publics

    #region Privates

    private async Task<IEnumerable<RoleModel>> GetRolesNonCachedAsync(IDbConnection dbConnection)
    {
        const string sql = $"SELECT [role].{nameof(Role.Id)}, [role].{nameof(Role.Type)} " +
                           "FROM [Roles] AS [role] " +
                           $"WHERE [role].{nameof(Role.IsDeleted)} <> 1 AND [role].{nameof(Role.IsHidden)} <> 1";
        var roles = await dbConnection.QueryAsync<RoleModel>(sql);

        _logger.LogInformation("Roles are retrieved from database directly.");

        return roles;
    }

    private async Task<IEnumerable<RoleModel>> GetRolesAsync(IDbConnection dbConnection, CancellationToken cancellationToken = default)
    {
        return await _cacheProvider.GetOrCreateAsync("config.roles",
            async _ => await GetRolesNonCachedAsync(dbConnection),
            new CacheOptions { Logger = _logger },
            cancellationToken);
    }

    #endregion Privates
}