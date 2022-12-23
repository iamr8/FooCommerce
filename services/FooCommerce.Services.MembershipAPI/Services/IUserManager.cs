using FooCommerce.Domain.Enums;
using FooCommerce.MembershipService.Contracts;
using FooCommerce.MembershipService.Dtos;
using FooCommerce.MembershipService.Enums;
using FooCommerce.MembershipService.Models;

namespace FooCommerce.MembershipService.Services;

public interface IUserManager
{
    Task<CreatedUserModel> CreateUserAsync(CreateUser model, RoleModel role);

    Task<RoleModel> GetRoleAsync(RoleType type, CancellationToken cancellationToken = default);

    Task<IEnumerable<RoleModel>> GetRolesAsync();

    Task<IEnumerable<UserInformationModel>> GetUserInformationAsync(Guid userId);

    Task<UserCredentialModel> GetUserModelAsync(CommType type, string value);

    Task<IEnumerable<UserSettingModel>> GetUserSettingsAsync(Guid userId);
}