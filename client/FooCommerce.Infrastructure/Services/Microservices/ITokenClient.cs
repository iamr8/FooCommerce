using FooCommerce.Infrastructure.Services.Microservices.Repositories;

namespace FooCommerce.Infrastructure.Services.Microservices;

public interface ITokenClient
{
    Task<DateTime?> GenerateAsync(Guid userCommunicationId, TimeSpan interval);
    Task<_TokenService.TokenStatus> ValidateAsync(Guid userCommunicationId, string code);
}