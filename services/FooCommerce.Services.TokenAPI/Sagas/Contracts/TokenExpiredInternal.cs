namespace FooCommerce.Services.TokenAPI.Sagas.Contracts;

public interface TokenExpiredInternal
{
    Guid CorrelationId { get; }
}