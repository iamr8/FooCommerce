using FooCommerce.Application.Communications.Enums;
using FooCommerce.MembershipAPI.Enums;
using FooCommerce.MembershipAPI.Models;

namespace FooCommerce.MembershipAPI.Services;

public interface IVerificationService
{
    Task<RequestVerificationResponse> RequestVerificationAsync(CommunicationType type, string value, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<string, object>> UpdateAuthTokenStateAsync(Guid authTokenId, AuthTokenState state, CancellationToken cancellationToken = default);
}