using FooCommerce.Application.Dtos.Membership;
using FooCommerce.Application.Enums.Membership;
using FooCommerce.Application.Models;
using FooCommerce.Application.Models.Membership;

namespace FooCommerce.Application.Services.Membership;

public interface IUserService
{
    ValueTask<RoleModel> GetRoleAsync(RoleType type, CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<RoleModel>> GetRolesAsync(CancellationToken cancellationToken = default);
    Task<SignInResponse> SignInAsync(SignInRequest model, string returnUrl = null, CancellationToken cancellationToken = default);
    Task<JobTaskResponse> SignUpAsync(SignUpRequest model, CancellationToken cancellationToken = default);
}