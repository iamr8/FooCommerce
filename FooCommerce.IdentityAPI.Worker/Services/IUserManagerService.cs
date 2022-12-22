using FooCommerce.IdentityAPI.Worker.Contracts;
using FooCommerce.IdentityAPI.Worker.Dtos;
using FooCommerce.IdentityAPI.Worker.Enums;
using FooCommerce.IdentityAPI.Worker.Models;

namespace FooCommerce.IdentityAPI.Worker.Services;

public interface IUserManagerService
{
    Task<CreatedUserModel> CreateUserAsync(SignUpRequest model, RoleModel role, CancellationToken cancellationToken = default);

    Task<RoleModel> GetRoleAsync(RoleType type, CancellationToken cancellationToken = default);

    Task<IEnumerable<RoleModel>> GetRolesAsync(CancellationToken cancellationToken = default);
}