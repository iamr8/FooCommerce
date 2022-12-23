namespace FooCommerce.Services.TokenAPI.Contracts;

public interface TokenGenerated
{
    Guid CorrelationId { get; }
    DateTime ExpiresOn { get; }
}