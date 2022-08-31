using FooCommerce.Application.Communications.Enums;
using FooCommerce.Domain.Enums;
using FooCommerce.MembershipAPI.Models;

namespace FooCommerce.Infrastructure.Services;

public interface IAccountService
{
    Task<JobStatus> RequestVerificationAsync(CommunicationType type, string value, CancellationToken cancellationToken = default);
    Task<SignInResponse> SignInAsync(SignInRequest model, string returnUrl = null, CancellationToken cancellationToken = default);
}