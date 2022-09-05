using System.Data;
using FooCommerce.IdentityAPI.Contracts.Requests;
using FooCommerce.IdentityAPI.Dtos;
using FooCommerce.IdentityAPI.Enums;
using FooCommerce.IdentityAPI.Worker.Models;

namespace FooCommerce.IdentityAPI.Worker.Services;

public interface IUserService
{
    Task<CreatedUserModel> CreateUserAsync(SignUpRequest model, RoleModel role, CancellationToken cancellationToken = default);
    ValueTask<RoleModel> GetRoleAsync(RoleType type, IDbConnection dbConnection, CancellationToken cancellationToken = default);
    ValueTask<IEnumerable<RoleModel>> GetRolesAsync(CancellationToken cancellationToken = default);
}