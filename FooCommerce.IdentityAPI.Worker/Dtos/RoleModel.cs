using FooCommerce.IdentityAPI.Worker.Enums;

namespace FooCommerce.IdentityAPI.Worker.Dtos;

public record RoleModel
{
    public Guid Id { get; init; }
    public RoleType Type { get; init; }
}