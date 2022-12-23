namespace FooCommerce.TokenService.Sagas.Contracts;

public interface TokenExpiredInternal
{
    Guid CorrelationId { get; }
}