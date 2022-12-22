using FooCommerce.Infrastructure.Services.Repositories;

namespace FooCommerce.Infrastructure.Services;

public interface ITokenService
{
    Task<DateTime?> GenerateAsync(Guid userCommunicationId, TimeSpan interval);
    Task<TokenService.TokenStatus> ValidateAsync(Guid userCommunicationId, string code);
}