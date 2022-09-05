using FooCommerce.IdentityAPI.Contracts.Requests;

namespace FooCommerce.Infrastructure.Services;

public interface IAccountService
{
    Task<JobStatus> RequestVerificationAsync(string auth, CancellationToken cancellationToken = default);
    Task<JobStatus> SignInAsync(SignInRequest model, CancellationToken cancellationToken = default);

    Task<JobStatus> SignUpAsync(SignUpRequest model, CancellationToken cancellationToken = default);
}