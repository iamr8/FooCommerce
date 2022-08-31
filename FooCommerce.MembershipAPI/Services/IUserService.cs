using FooCommerce.MembershipAPI.Dtos;
using FooCommerce.MembershipAPI.Enums;
using FooCommerce.MembershipAPI.Models;

namespace FooCommerce.MembershipAPI.Services;

public interface IUserService
{
    ValueTask<RoleModel> GetRoleAsync(RoleType type, CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<RoleModel>> GetRolesAsync(CancellationToken cancellationToken = default);

    Task<SignInResponse> SignInAsync(SignInRequest model, string returnUrl = null, CancellationToken cancellationToken = default);

    Task<SignUpResponse> SignUpAsync(SignUpRequest model, CancellationToken cancellationToken = default);
}