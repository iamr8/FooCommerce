using FooCommerce.Application.Dtos.Membership;
using FooCommerce.Application.Enums.Membership;
using FooCommerce.Application.Models.Membership;

namespace FooCommerce.Application.Services.Membership;

public interface IUserService
{
    ValueTask<RoleModel> GetRoleAsync(RoleTypes type, CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<RoleModel>> GetRolesAsync(CancellationToken cancellationToken = default);

    Task<SignUpResponse> SignUpAsync(SignUpRequest model, CancellationToken cancellationToken = default);
}