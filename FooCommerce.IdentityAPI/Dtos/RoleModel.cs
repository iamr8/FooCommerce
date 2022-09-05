using FooCommerce.IdentityAPI.Enums;

namespace FooCommerce.IdentityAPI.Dtos;

public record RoleModel
{
    public Guid Id { get; init; }
    public RoleType Type { get; init; }
}