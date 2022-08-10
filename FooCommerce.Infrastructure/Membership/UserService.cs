using System.Data;

using Dapper;

using FooCommerce.Application.Entities.Membership;
using FooCommerce.Application.Enums.Membership;
using FooCommerce.Application.Models.Membership;
using FooCommerce.Application.Services.Listings;
using FooCommerce.Application.Services.Membership;
using FooCommerce.Domain.Enums;
using FooCommerce.Domain.Services;
using FooCommerce.Infrastructure.Caching;
using FooCommerce.Infrastructure.Membership.Dtos;
using FooCommerce.Infrastructure.Membership.Validators;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace FooCommerce.Infrastructure.Membership;

public class UserService : IUserService
{
    private readonly IDbConnection _dbConnection;
    private readonly IPasswordProtectorService _protectorService;
    private readonly ILocationService _locationService;
    private readonly ILogger<IUserService> _logger;
    private readonly IMemoryCache _memoryCache;

    public UserService(IDbConnection dbConnection, IPasswordProtectorService protectorService, ILocationService locationService, ILogger<IUserService> logger, IMemoryCache memoryCache)
    {
        _locationService = locationService;
        _logger = logger;
        _memoryCache = memoryCache;
        _dbConnection = dbConnection;
        _protectorService = protectorService;
    }

    public async Task<bool> CheckIfEmailAlreadyEstablishedAsync(string email)
    {
        var establishedId = await _dbConnection.QuerySingleAsync<Guid>("SELECT TOP(1) [communication].Id" +
                                                                    "FROM [UserCommunications] AS [communication]" +
                                                                    "WHERE [communication].IsVerified = 1 AND [communication].Type = 0 AND [communication].Value = N'@Email'",
            new { Email = email.ToLowerInvariant() });

        return establishedId != Guid.Empty;
    }

    private async ValueTask<IEnumerable<RoleModel>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync<IEnumerable<RoleModel>>("high.config.roles", async () =>
        {
            var roles = (await _dbConnection.QueryAsync<RoleModel>("SELECT [role].Id, [role].Type" +
                                                                   "FROM [Roles] AS [role]" +
                                                                   "WHERE [role].IsDeleted <> 1 AND [role].IsHidden <> 1"))
                .AsList();
            return roles;
        }, cancellationToken: cancellationToken);
    }

    private async ValueTask<RoleModel> GetRoleAsync(RoleTypes type, CancellationToken cancellationToken = default)
    {
        var roles = await GetRolesAsync(cancellationToken);
        var role = roles.Single(x => x.Type == type);
        return role;
    }

    public async Task CreateUserAsync(SignUpRequest model, RoleModel role)
    {
        // Must do this by Event Bus
        var user = await _dbConnection.ExecuteScalarAsync<User>("INSERT INTO [Users] () VALUES ()");

        var hash = _protectorService.Hash(model.Password);
        var userPassword = await _dbConnection.ExecuteScalarAsync<UserPassword>(
            "INSERT INTO [UserPasswords] ([Hash], [UserId]) VALUES (@Hash, @UserId)", new
            {
                Hash = hash,
                UserId = user.Id
            });

        var userRole = await _dbConnection.ExecuteScalarAsync<UserRole>(
            "INSERT INTO [UserRoles] ([UserId], [RoleId]) VALUES (@RoleId, @UserId)", new
            {
                RoleId = role.Id,
                UserId = user.Id
            });

        var userCommunication = await _dbConnection.ExecuteScalarAsync<UserCommunication>(
            "INSERT INTO [UserCommunications] ([Type], [Value], [UserId]) VALUES (@Type, @Value, @UserId)", new
            {
                Type = (ushort)UserCommunicationTypes.Email,
                Value = model.Email.ToLowerInvariant(),
                UserId = user.Id
            });

        var userInformation = await _dbConnection.ExecuteScalarAsync<IEnumerable<UserInformation>>(
            "INSERT INTO [UserInformation] ([Type], [Value], [UserId]) VALUES (@Type, @Value, @UserId)", new
            {
                Type = new[] { (ushort)UserInformationTypes.Name, (ushort)UserInformationTypes.Surname },
                Value = new[] { model.FirstName, model.LastName },
                UserId = new[] { user.Id, user.Id }
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

        var isEstablished = await CheckIfEmailAlreadyEstablishedAsync(model.Email);
        if (isEstablished)
            return JobStatus.EmailAlreadyEstablished;

        var role = await GetRoleAsync(RoleTypes.NormalUser, cancellationToken);
        await CreateUserAsync(model, role);

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