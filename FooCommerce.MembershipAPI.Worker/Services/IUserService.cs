using System.Data;
using FooCommerce.MembershipAPI.Contracts.Requests;
using FooCommerce.MembershipAPI.Dtos;
using FooCommerce.MembershipAPI.Enums;
using FooCommerce.MembershipAPI.Worker.Models;

namespace FooCommerce.MembershipAPI.Worker.Services;

public interface IUserService
{
    Task<CreatedUserModel> CreateUserAsync(SignUpRequest model, RoleModel role, CancellationToken cancellationToken = default);
    ValueTask<RoleModel> GetRoleAsync(RoleType type, IDbConnection dbConnection, CancellationToken cancellationToken = default);
}