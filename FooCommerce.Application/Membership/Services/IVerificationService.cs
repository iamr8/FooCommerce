using FooCommerce.Application.Communications.Enums;
using FooCommerce.Domain.Enums;

namespace FooCommerce.Application.Membership.Services;

public interface IVerificationService
{
    Task<JobStatus> RequestVerificationAsync(CommunicationType type, string value, CancellationToken cancellationToken = default);
}