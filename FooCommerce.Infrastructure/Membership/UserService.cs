using System.Data;

using Dapper;

using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Dtos.Membership;
using FooCommerce.Application.Entities.Membership;
using FooCommerce.Application.Enums.Membership;
using FooCommerce.Application.Models.Membership;
using FooCommerce.Application.Services.Listings;
using FooCommerce.Application.Services.Membership;
using FooCommerce.Domain.Enums;
using FooCommerce.Infrastructure.Caching;
using FooCommerce.Infrastructure.Membership.Validators;
using FooCommerce.Infrastructure.Protection;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace FooCommerce.Infrastructure.Membership;

public class UserService : IUserService
{
    private readonly ILocationService _locationService;
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<IUserService> _logger;

    public UserService(ILocationService locationService, IDbConnectionFactory dbConnectionFactory, IMemoryCache memoryCache, ILogger<IUserService> logger)
    {
        _locationService = locationService;
        _logger = logger;
        _dbConnectionFactory = dbConnectionFactory;
        _memoryCache = memoryCache;
    }

    private async Task<bool> CheckIfEmailAlreadyEstablishedAsync(string email, IDbConnection dbConnection)
    {
        var establishedIds = await dbConnection.QueryAsync<Guid>($"SELECT TOP(1) [communication].{nameof(UserCommunication.Id)} " +
                                                                 "FROM [UserCommunications] AS [communication] " +
                                                                 $"WHERE [communication].{nameof(UserCommunication.IsVerified)} = 1 AND [communication].{nameof(UserCommunication.Type)} = 0 AND [communication].{nameof(UserCommunication.Value)} = N'@Email'",
            new { Email = email.ToLowerInvariant() });
        if (establishedIds == null || !establishedIds.Any())
            return false;

        var establishedId = establishedIds.Single();
        return establishedId != Guid.Empty;
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
        return await _memoryCache.GetOrCreateAsync("config.roles",
            async () => await GetRolesNonCachedAsync(dbConnection), _logger, cancellationToken: cancellationToken);
    }

    public async ValueTask<IEnumerable<RoleModel>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        using var dbConnection = _dbConnectionFactory.CreateConnection();

        return await GetRolesAsync(dbConnection, cancellationToken);
    }

    public async ValueTask<RoleModel> GetRoleAsync(RoleTypes type, CancellationToken cancellationToken = default)
    {
        using var dbConnection = _dbConnectionFactory.CreateConnection();

        return await GetRoleAsync(type, dbConnection, cancellationToken);
    }

    private async ValueTask<RoleModel> GetRoleAsync(RoleTypes type, IDbConnection dbConnection, CancellationToken cancellationToken = default)
    {
        var roles = await GetRolesAsync(dbConnection, cancellationToken);
        var role = roles.Single(x => x.Type == type);
        return role;
    }

    private async Task CreateUserAsync(SignUpRequest model, RoleModel role, IDbConnection dbConnection)
    {
        // Must do this by Event Bus
        var user = await dbConnection.QuerySingleAsync<User>("INSERT INTO [Users] OUTPUT INSERTED.* DEFAULT VALUES");

        var hash = DataProtector.Hash(model.Password, 16, 32, 10_000);
        var userPassword = await dbConnection.QuerySingleAsync<UserPassword>(
            $"INSERT INTO [UserPasswords] ([{nameof(UserPassword.Hash)}], [{nameof(UserPassword.UserId)}]) OUTPUT INSERTED.* VALUES (@Hash, @UserId)", new
            {
                Hash = hash,
                UserId = user.Id,
            });

        var userRole = await dbConnection.QuerySingleAsync<UserRole>(
            $"INSERT INTO [UserRoles] ([{nameof(UserRole.UserId)}], [{nameof(UserRole.RoleId)}]) OUTPUT INSERTED.* VALUES (@UserId, @RoleId)", new
            {
                RoleId = role.Id,
                UserId = user.Id
            });

        var userCommunication = await dbConnection.QuerySingleAsync<UserCommunication>(
            $"INSERT INTO [UserCommunications] ([{nameof(UserCommunication.Type)}], [{nameof(UserCommunication.Value)}], [{nameof(UserCommunication.UserId)}]) OUTPUT INSERTED.* VALUES (@Type, @Value, @UserId)", new
            {
                Type = (byte)UserCommunicationTypes.Email,
                Value = model.Email.ToLowerInvariant(),
                UserId = user.Id
            });

        var userFirstName = await dbConnection.QuerySingleAsync<UserInformation>(
            $"INSERT INTO [UserInformation] ([{nameof(UserInformation.Type)}], [{nameof(UserInformation.Value)}], [{nameof(UserInformation.UserId)}]) OUTPUT INSERTED.* VALUES (@Type, @Value, @UserId)", new
            {
                Type = (byte)UserInformationTypes.Name,
                Value = model.FirstName,
                UserId = user.Id
            });

        var userLastName = await dbConnection.QuerySingleAsync<UserInformation>(
            $"INSERT INTO [UserInformation] ([{nameof(UserInformation.Type)}], [{nameof(UserInformation.Value)}], [{nameof(UserInformation.UserId)}]) OUTPUT INSERTED.* VALUES (@Type, @Value, @UserId)", new
            {
                Type = (byte)UserInformationTypes.Surname,
                Value = model.LastName,
                UserId = user.Id
            });
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

        var isEstablished = await CheckIfEmailAlreadyEstablishedAsync(model.Email, dbConnection);
        if (isEstablished)
            return JobStatus.EmailAlreadyEstablished;

        var role = await GetRoleAsync(RoleTypes.NormalUser, dbConnection, cancellationToken);
        await CreateUserAsync(model, role, dbConnection);
        return JobStatus.Success;

        //var query = from user in dbContext.Set<User>().AsNoTracking()
        //    join password in dbContext.Set<UserPassword>().AsNoTracking() on user.Id equals password.UserId into
        //        passwords
        //    join communication in dbContext.Set<UserCommunication>().AsNoTracking() on user.Id equals communication
        //        .UserId into communications
        //    join lockout in dbContext.Set<UserLockout>().AsNoTracking() on user.Id equals lockout.UserId into lockouts
        //    join role in dbContext.Set<UserRole>().AsNoTracking() on user.Id equals role.UserId into roles
        //    join setting in dbContext.Set<UserSetting>().AsNoTracking() on user.Id equals setting.UserId into settings
        //    join information in dbContext.Set<UserInformation>().AsNoTracking() on user.Id equals information.UserId
        //        into informations
        //    select user;
    }
}