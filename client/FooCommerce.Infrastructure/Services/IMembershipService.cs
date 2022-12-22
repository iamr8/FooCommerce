using FooCommerce.Infrastructure.Membership.Contracts;

namespace FooCommerce.Infrastructure.Services;

public interface IMembershipService
{
    // Task<JobStatus> FulfillVerificationAsync(string token, TokenRequestPurpose purpose, CancellationToken cancellationToken = default);

    // Task<JobStatus> RequestVerificationAsync(string auth, CancellationToken cancellationToken = default);

    // Task<JobStatus> SignInAsync(SignInRequest model, CancellationToken cancellationToken = default);

    // Task<JobStatus> SignUpAsync(SignUpRequest model, CancellationToken cancellationToken = default);
    Task<Guid> RegisterAsync(SignUpRequest model);
}