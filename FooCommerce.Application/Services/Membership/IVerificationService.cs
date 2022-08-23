using FooCommerce.Application.Enums.Membership;
using FooCommerce.Domain.Enums;

namespace FooCommerce.Application.Services.Membership;

public interface IVerificationService
{
    Task<JobStatus> RequestVerificationAsync(CommunicationType type, string value, CancellationToken cancellationToken = default);
}