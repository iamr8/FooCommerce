using FooCommerce.Domain.Enums;
using FooCommerce.Services.MembershipAPI.Contracts;
using FooCommerce.Services.MembershipAPI.Dtos;
using FooCommerce.Services.MembershipAPI.Enums;
using FooCommerce.Services.MembershipAPI.Models;

namespace FooCommerce.Services.MembershipAPI.Services;

public interface IUserManager
{
    Task<CreatedUserModel> CreateUserAsync(CreateUser model, RoleModel role);

    Task<RoleModel> GetRoleAsync(RoleType type, CancellationToken cancellationToken = default);

    Task<IEnumerable<RoleModel>> GetRolesAsync();

    Task<IEnumerable<UserInformationModel>> GetUserInformationAsync(Guid userId);

    Task<UserCredentialModel> GetUserModelAsync(CommType type, string value);

    Task<IEnumerable<UserSettingModel>> GetUserSettingsAsync(Guid userId);
}