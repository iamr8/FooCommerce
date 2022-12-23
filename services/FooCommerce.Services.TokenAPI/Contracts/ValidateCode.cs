namespace FooCommerce.TokenService.Contracts;

public interface ValidateCode
{
    Guid CorrelationId { get; }
    string Code { get; }
}