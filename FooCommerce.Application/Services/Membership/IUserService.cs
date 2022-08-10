using FooCommerce.Application.Models.Membership;

namespace FooCommerce.Application.Services.Membership;

public interface IUserService
{
    Task<SignUpResponse> SignUpAsync(SignUpRequest model, CancellationToken cancellationToken = default);
}