using FooCommerce.IdentityAPI.Worker.Contracts.Enums;
using FooCommerce.IdentityAPI.Worker.Enums;

namespace FooCommerce.IdentityAPI.Worker.Contracts;

public interface UserCreationFaulted
{
    UserCreationFault Fault { get; }
}