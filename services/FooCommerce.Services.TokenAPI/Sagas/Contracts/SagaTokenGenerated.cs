using FooCommerce.Services.TokenAPI.Interfaces;

namespace FooCommerce.Services.TokenAPI.Sagas.Contracts;

public interface SagaTokenGenerated : IIdentifier
{
    string Code { get; }
    DateTime GeneratedOn { get; }
}