using FooCommerce.Services.TokenAPI.Interfaces;

namespace FooCommerce.Services.TokenAPI.Contracts;

public interface GenerateCode : IIdentifier
{
    int LifetimeInSeconds { get; }
}