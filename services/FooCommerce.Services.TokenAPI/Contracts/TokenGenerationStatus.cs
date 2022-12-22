namespace FooCommerce.Services.TokenAPI.Contracts;

public interface TokenGenerationStatus
{
    DateTime ExpiresOn { get; }
}