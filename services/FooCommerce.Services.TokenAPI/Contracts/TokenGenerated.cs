namespace FooCommerce.TokenService.Contracts;

public interface TokenGenerated
{
    Guid CorrelationId { get; }
    DateTime ExpiresOn { get; }
}