using FooCommerce.Application.Membership.Dtos;
using FooCommerce.Application.Membership.Enums;
using FooCommerce.Application.Membership.Models;
using FooCommerce.Application.Models;

namespace FooCommerce.Application.Membership.Services;

public interface IUserService
{
    ValueTask<RoleModel> GetRoleAsync(RoleType type, CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<RoleModel>> GetRolesAsync(CancellationToken cancellationToken = default);
    Task<SignInResponse> SignInAsync(SignInRequest model, string returnUrl = null, CancellationToken cancellationToken = default);
    Task<JobTaskResponse> SignUpAsync(SignUpRequest model, CancellationToken cancellationToken = default);
}