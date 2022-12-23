using System.Globalization;
using System.Security.Claims;
using FooCommerce.Common.Protection;
using FooCommerce.Domain.Enums;
using FooCommerce.MembershipService.Contracts;
using FooCommerce.MembershipService.DbProvider;
using FooCommerce.MembershipService.DbProvider.Entities;
using FooCommerce.MembershipService.Dtos;
using FooCommerce.MembershipService.Enums;
using FooCommerce.MembershipService.Models;
using Microsoft.EntityFrameworkCore;

namespace FooCommerce.MembershipService.Services.Repositories;

public class UserManager : IUserManager
{
    private readonly IDbContextFactory<MembershipDbContext> _dbContextFactory;

    public UserManager(IDbContextFactory<MembershipDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    #region Statics

    public async Task<IEnumerable<UserSettingModel>> GetUserSettingsAsync(Guid userId)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var userSettings = await dbContext.UserSettings
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.UserId == userId)
            .Select(x => new UserSettingModel
            {
                Key = x.Key,
                Value = x.Value
            })
            .ToListAsync();
        return userSettings;
    }

    public async Task<IEnumerable<UserInformationModel>> GetUserInformationAsync(Guid userId)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var informationModels = await dbContext.UserInformation
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.UserId == userId)
            .Select(x => new UserInformationModel
            {
                Type = x.Type,
                Value = x.Value
            })
            .ToListAsync();
        return informationModels;
    }

    public async Task<UserCredentialModel> GetUserModelAsync(CommType type, string value)
    {
        var _value = value.ToLowerInvariant();
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var credentialModel = await dbContext.UserCommunications
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.User.Roles.OrderByDescending(c => c.Created).Take(1)).ThenInclude(x => x.Role)
            .Include(x => x.User.Passwords.OrderByDescending(c => c.Created).Take(1))
            .Where(uc => uc.IsVerified && uc.Type == type && uc.Value == _value)
            .Select(uc => new UserCredentialModel
            {
                UserId = uc.UserId,
                Hash = uc.User.Passwords.First().Hash,
                CommunicationId = uc.Id,
                RoleId = uc.User.Roles.First().Id,
                RoleType = uc.User.Roles.First().Role.Type,
            })
            .FirstOrDefaultAsync();
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

    public async Task<RoleModel> GetRoleAsync(RoleType type, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var role = await dbContext.Roles
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => !x.IsDeleted && !x.IsInvisible && x.Type == type)
            .Select(x => new RoleModel
            {
                Id = x.Id,
                Type = x.Type
            })
            .SingleOrDefaultAsync();
        return role;
    }

    public async Task<CreatedUserModel> CreateUserAsync(CreateUser model, RoleModel role)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var output = new CreatedUserModel();

        var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            output.User = dbContext.Users.Add(new User()).Entity;
            await dbContext.SaveChangesAsync();

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
                Type = CommType.Email,
                Value = model.Email.ToLowerInvariant(),
                UserId = output.User.Id
            }).Entity;

            //output.Settings ??= new List<UserSetting>();
            //output.Settings.Add(dbContext.UserSettings.Add(new UserSetting
            //{
            //    Key = UserSettingKey.Country,
            //    Value = model.Country.ToString(),
            //    UserId = output.User.Id
            //}).Entity);

            output.Information ??= new List<UserInformation>();
            output.Information.Add(dbContext.UserInformation.Add(new UserInformation
            {
                Type = UserInformationType.Name,
                Value = model.FirstName,
                UserId = output.User.Id
            }).Entity);

            output.Information.Add(dbContext.UserInformation.Add(new UserInformation
            {
                Type = UserInformationType.Surname,
                Value = model.LastName,
                UserId = output.User.Id
            }).Entity);

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return output;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    #endregion Publics

    #region Privates

    public async Task<IEnumerable<RoleModel>> GetRolesAsync()
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var roles = await dbContext.Roles
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => !x.IsDeleted && !x.IsInvisible)
            .Select(x => new RoleModel
            {
                Id = x.Id,
                Type = x.Type
            })
            .ToListAsync();
        return roles;
    }

    #endregion Privates
}