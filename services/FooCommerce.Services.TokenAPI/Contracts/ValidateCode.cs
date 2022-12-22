using FooCommerce.Services.TokenAPI.Interfaces;

namespace FooCommerce.Services.TokenAPI.Contracts;

public interface ValidateCode : IIdentifier
{
    string Code { get; }
}