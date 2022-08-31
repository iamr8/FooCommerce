using FooCommerce.Domain.Enums;
using FooCommerce.MembershipAPI.Models;

namespace FooCommerce.Infrastructure.Services;

public interface IAccountService
{
    Task<JobStatus> SignInAsync(SignInRequest model, string returnUrl = null, CancellationToken cancellationToken = default);
    Task<JobStatus> SignUpAsync(SignUpRequest model, CancellationToken cancellationToken = default);
}