using FooCommerce.Services.TokenAPI.Interfaces;

namespace FooCommerce.Services.TokenAPI.Contracts;

public interface ValidateCode
{
    Guid CorrelationId { get; }
    string Code { get; }
}