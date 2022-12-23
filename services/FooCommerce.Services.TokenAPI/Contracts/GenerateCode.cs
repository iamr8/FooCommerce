using FooCommerce.TokenService.Interfaces;

namespace FooCommerce.TokenService.Contracts;

public interface GenerateCode : IIdentifier
{
    int LifetimeInSeconds { get; }
}