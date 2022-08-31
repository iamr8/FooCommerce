using FooCommerce.Application.Communications.Enums;
using FooCommerce.MembershipAPI.Models;

namespace FooCommerce.MembershipAPI.Services;

public interface IVerificationService
{
    Task<RequestVerificationResponse> RequestVerificationAsync(CommunicationType type, string value, CancellationToken cancellationToken = default);
}