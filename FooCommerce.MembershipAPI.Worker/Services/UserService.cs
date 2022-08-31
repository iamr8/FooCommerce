using System.Data;
using System.Security.Claims;

using Dapper;

using EasyCaching.Core;

using FooCommerce.Application.Communications.Enums;
using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Localization.Services;
using FooCommerce.Core.Caching;
using FooCommerce.Core.Protection;
using FooCommerce.Domain.Enums;
using FooCommerce.MembershipAPI.Dtos;
using FooCommerce.MembershipAPI.Enums;
using FooCommerce.MembershipAPI.Models;
using FooCommerce.MembershipAPI.Services;
using FooCommerce.MembershipAPI.Worker.DbProvider.Entities;
using FooCommerce.MembershipAPI.Worker.Enums;
using FooCommerce.MembershipAPI.Worker.Models;
using FooCommerce.MembershipAPI.Worker.Validators;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;

namespace FooCommerce.MembershipAPI.Worker.Services;

public class UserService : IUserService
{
    private readonly ILocationService _locationService;
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IEasyCachingProvider _easyCaching;
    private readonly ILogger<IUserService> _logger;

    public UserService(ILocationService locationService, IDbConnectionFactory dbConnectionFactory, IEasyCachingProvider easyCaching, ILogger<IUserService> logger)
    {
        _locationService = locationService;
        _logger = logger;
        _dbConnectionFactory = dbConnectionFactory;
        _easyCaching = easyCaching;
    }

    private async Task<Guid?> CheckIfUsernameAlreadyEstablishedAsync(CommunicationType type, string value, IDbConnection dbConnection)
    {
        var establishedIds = await dbConnection.QueryAsync<Guid>($"SELECT TOP(1) [communication].{nameof(UserCommunication.Id)} " +
                                                                 "FROM [UserCommunications] AS [communication] " +
                                                                 $"WHERE [communication].{nameof(UserCommunication.IsVerified)} = 1 AND [communication].{nameof(UserCommunication.Type)} = {(byte)type} AND [communication].{nameof(UserCommunication.Value)} = N'@Value'",
            new { Value = value.ToLowerInvariant() });
        if (establishedIds == null || !establishedIds.Any())
            return null;

        var establishedId = establishedIds.Single();
        return establishedId != Guid.Empty ? establishedId : null;
    }

    private async Task GetUserModelAsync(UserModel model, IDbConnection dbConnection,
        CancellationToken cancellationToken = default)
    {
        var users = await dbConnection.QueryAsync($"SELECT TOP(1) [user].{nameof(User.Id)} AS [UserId], [role].{nameof(Role.Type)} AS [Role], [userRole].{nameof(UserRole.RoleId)} " +
                                                             $"FROM [Users] AS [user]" +
                                                             // $"--OUTER APPLY (SELECT [userInformation].Type, [userInformation].Value FROM [UserInformation] AS [userInformation] WHERE [userInformation].UserId = [user].Id ORDER BY [userInformation].Created DESC) AS [userInformation]" +
                                                             // $"--OUTER APPLY (SELECT [userCommunication].Type, [userCommunication].Value FROM [UserCommunications] AS [userCommunication] WHERE [userCommunication].UserId = [user].Id AND [userCommunication].IsVerified = 1 ORDER BY [userCommunication].Created DESC) AS [userCommunication]" +
                                                             // $"-- CROSS APPLY (SELECT TOP(1) [userSetting].[{nameof(UserSetting.Key)}], [userSetting].{nameof(UserSetting.Value)} FROM [UserSettings] AS [userSetting] WHERE [userSetting].{nameof(UserSetting.UserId)} = [user].{nameof(User.Id)} ORDER BY [userSetting].{nameof(UserSetting.Created)} DESC) AS [userSetting]" +
                                                             $"CROSS APPLY (SELECT TOP(1) [userRole].{nameof(UserRole.RoleId)} FROM [UserRoles] AS [userRole] WHERE [userRole].{nameof(UserRole.UserId)} = [user].{nameof(User.Id)} ORDER BY [userRole].{nameof(UserRole.Created)} DESC) AS [userRole]" +
                                                             $"CROSS APPLY (SELECT TOP(1) [role].{nameof(Role.Type)} FROM [Roles] AS [role] WHERE [role].{nameof(Role.Id)} = [userRole].{nameof(UserRole.RoleId)}) AS [role]" +
                                                             $"WHERE [user].{nameof(User.Id)} = '@UserId'",
            new { UserId = model.Id });
        if (users == null || !users.Any())
            throw new NullReferenceException("Unable to find a user with given Id.");

        var user = users.Single();

        model.Role = new RoleModel
        {
            Id = user.RoleId,
            Type = user.Role
        };
    }

    private async Task<UserCredentialModel> GetUserCredentialAsync(CommunicationType type, string value, IDbConnection dbConnection)
    {
        var users = await dbConnection.QueryAsync<UserCredentialModel>($"SELECT TOP(1) [user].{nameof(User.Id)} AS [UserId], [password].{nameof(UserPassword.Hash)} " +
                                                                 "FROM [UserCommunications] AS [communication] " +
                                                                 $"CROSS APPLY (SELECT TOP(1) [user].{nameof(User.Id)} FROM [Users] as [user] WHERE [user].{nameof(User.Id)} = [communication].{nameof(UserCommunication.UserId)}) AS [user] " +
                                                                 $"CROSS APPLY (SELECT TOP(1) [password].{nameof(UserPassword.Hash)} FROM [UserPasswords] AS [password] WHERE [password].{nameof(UserPassword.UserId)} = [user].{nameof(User.Id)} ORDER BY [password].{nameof(UserPassword.Created)} DESC) AS [password] " +
                                                                 $"WHERE [communication].{nameof(UserCommunication.IsVerified)} = 1 AND [communication].{nameof(UserCommunication.Type)} = {(byte)type} AND [communication].{nameof(UserCommunication.Value)} = N'@Value'",
             new { Value = value.ToLowerInvariant() });
        if (users == null || !users.Any())
            return null;

        var user = users.Single();
        return user;
    }

    public async Task<IEnumerable<RoleModel>> GetRolesNonCachedAsync(IDbConnection dbConnection)
    {
        var roles = await dbConnection.QueryAsync<RoleModel>($"SELECT [role].{nameof(Role.Id)}, [role].{nameof(Role.Type)} " +
                                                             "FROM [Roles] AS [role] " +
                                                             $"WHERE [role].{nameof(Role.IsDeleted)} <> 1 AND [role].{nameof(Role.IsHidden)} <> 1");

        _logger.LogInformation("Roles are retrieved from database directly.");

        return roles;
    }

    private async ValueTask<IEnumerable<RoleModel>> GetRolesAsync(IDbConnection dbConnection, CancellationToken cancellationToken = default)
    {
        return await _easyCaching.GetOrCreateAsync("config.roles",
            async () => await GetRolesNonCachedAsync(dbConnection), _logger, cancellationToken: cancellationToken);
    }

    public async ValueTask<IEnumerable<RoleModel>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        using var dbConnection = _dbConnectionFactory.CreateConnection();

        return await GetRolesAsync(dbConnection, cancellationToken);
    }

    public async ValueTask<RoleModel> GetRoleAsync(RoleType type, CancellationToken cancellationToken = default)
    {
        using var dbConnection = _dbConnectionFactory.CreateConnection();

        return await GetRoleAsync(type, dbConnection, cancellationToken);
    }

    private async ValueTask<RoleModel> GetRoleAsync(RoleType type, IDbConnection dbConnection, CancellationToken cancellationToken = default)
    {
        var roles = await GetRolesAsync(dbConnection, cancellationToken);
        var role = roles.Single(x => x.Type == type);
        return role;
    }

    private async Task<CreatedUserModel> CreateUserAsync(SignUpRequest model, RoleModel role, IDbConnection dbConnection)
    {
        // Must use DbContext Transaction and Rollback strategy
        var output = new CreatedUserModel();
        output.User = await dbConnection.QuerySingleAsync<User>("INSERT INTO [Users] OUTPUT INSERTED.* DEFAULT VALUES");

        var hash = DataProtector.Hash(model.Password, 16, 32, 10_000);
        output.Password = await dbConnection.QuerySingleAsync<UserPassword>(
            $"INSERT INTO [UserPasswords] ([{nameof(UserPassword.Hash)}], [{nameof(UserPassword.UserId)}]) OUTPUT INSERTED.* VALUES (@Hash, @UserId)", new
            {
                Hash = hash,
                UserId = output.User.Id,
            });

        output.Role = await dbConnection.QuerySingleAsync<UserRole>(
            $"INSERT INTO [UserRoles] ([{nameof(UserRole.UserId)}], [{nameof(UserRole.RoleId)}]) OUTPUT INSERTED.* VALUES (@UserId, @RoleId)", new
            {
                RoleId = role.Id,
                UserId = output.User.Id
            });

        output.Communication = await dbConnection.QuerySingleAsync<UserCommunication>(
            $"INSERT INTO [UserCommunications] ([{nameof(UserCommunication.Type)}], [{nameof(UserCommunication.Value)}], [{nameof(UserCommunication.UserId)}]) OUTPUT INSERTED.* VALUES (@Type, @Value, @UserId)", new
            {
                Type = (byte)CommunicationType.Email_Message,
                Value = model.Email.ToLowerInvariant(),
                UserId = output.User.Id
            });

        output.Settings ??= new List<UserSetting>();
        output.Settings.Add(await dbConnection.QuerySingleAsync<UserSetting>(
            $"INSERT INTO [UserSettings] ([{nameof(UserSetting.Key)}], [{nameof(UserSetting.Value)}], [{nameof(UserSetting.UserId)}]) OUTPUT INSERTED.* VALUES (@Key, @Value, @UserId)", new
            {
                Key = "country",
                Value = model.Country.ToString(),
                UserId = output.User.Id
            }));

        output.Information ??= new List<UserInformation>();
        output.Information.Add(await dbConnection.QuerySingleAsync<UserInformation>(
            $"INSERT INTO [UserInformation] ([{nameof(UserInformation.Type)}], [{nameof(UserInformation.Value)}], [{nameof(UserInformation.UserId)}]) OUTPUT INSERTED.* VALUES (@Type, @Value, @UserId)", new
            {
                Type = (byte)UserInformationType.Name,
                Value = model.FirstName,
                UserId = output.User.Id
            }));

        output.Information.Add(await dbConnection.QuerySingleAsync<UserInformation>(
            $"INSERT INTO [UserInformation] ([{nameof(UserInformation.Type)}], [{nameof(UserInformation.Value)}], [{nameof(UserInformation.UserId)}]) OUTPUT INSERTED.* VALUES (@Type, @Value, @UserId)", new
            {
                Type = (byte)UserInformationType.Surname,
                Value = model.LastName,
                UserId = output.User.Id
            }));

        return output;
    }

    public async Task<SignInResponse> SignInAsync(SignInRequest model, string returnUrl = null, CancellationToken cancellationToken = default)
    {
        var validator = new SignInRequestValidator();
        var validationResult = await validator.ValidateAsync(model, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new SignInResponse
            {
                Status = JobStatus.InputDataNotValid,
                Errors = validationResult.Errors,
            };
        }

        using var dbConnection = _dbConnectionFactory.CreateConnection();

        var usernameType = model.Username.Contains('@') ? CommunicationType.Email_Message : CommunicationType.Mobile_Sms;
        var credentialModel = await GetUserCredentialAsync(usernameType, model.Username, dbConnection);
        if (credentialModel == null)
            return JobStatus.IncorrectUsernameOrPassword;

        var (verified, needsUpgrade) = DataProtector.Check(credentialModel.Hash, model.Password, 32, 10_000);
        if (!verified)
            return JobStatus.IncorrectUsernameOrPassword;

        // TODO: we check if user's communication is verified or not
        // If communication hasn't verified yet, we return NeedVerification status
        // Otherwise, we create ClaimsPrincipal and Sign them in

        //if (needsUpgrade)
        //    return JobStatus.UpgradePassword;
        var userModel = new UserModel
        {
            Id = credentialModel.UserId,
            Hash = credentialModel.Hash,
        };
        await GetUserModelAsync(userModel, dbConnection, cancellationToken);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, model.Username),
            new(ClaimTypes.Email, userModel.Email),
            new(ClaimTypes.MobilePhone, userModel.Mobile),
            new(ClaimTypes.GivenName, userModel.Name),
            new(ClaimTypes.Surname, userModel.Surname),
            new(ClaimTypes.Country, userModel.Country.TwoLetterISORegionName),
            new(ClaimTypes.Role, userModel.Role.Type.ToString(),typeof(RoleType).ToString()),
            new("RoleId", userModel.Role.Id.ToString("N"), typeof(Guid).ToString()),
            new(ClaimTypes.NameIdentifier, userModel.Id.ToString("N"), typeof(Guid).ToString()),
            new(ClaimTypes.Hash, credentialModel.Hash)
        };
        var claimsIdentity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var authenticationProps = new AuthenticationProperties
        {
            AllowRefresh = true,
            IsPersistent = model.Remember,
            RedirectUri = returnUrl,
            IssuedUtc = DateTimeOffset.UtcNow,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
        };

        var output = new SignInResponse(claimsPrincipal, authenticationProps);
        return output;
    }

    public async Task<SignUpResponse> SignUpAsync(SignUpRequest model, CancellationToken cancellationToken = default)
    {
        var validator = new SignUpRequestValidator(_locationService);
        var validationResult = await validator.ValidateAsync(model, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new SignUpResponse
            {
                Status = JobStatus.InputDataNotValid,
                Errors = validationResult.Errors
            };
        }

        using var dbConnection = _dbConnectionFactory.CreateConnection();

        var establishedId = await CheckIfUsernameAlreadyEstablishedAsync(CommunicationType.Email_Message, model.Email, dbConnection);
        if (establishedId != null)
            return JobStatus.EmailAlreadyEstablished;

        var role = await GetRoleAsync(RoleType.NormalUser, dbConnection, cancellationToken);
        var createdUserModel = await CreateUserAsync(model, role, dbConnection);
        return new SignUpResponse
        {
            Status = JobStatus.Success,
            CommunicationType = createdUserModel.Communication.Type,
            CommunicationAddress = createdUserModel.Communication.Value
        };
    }
}