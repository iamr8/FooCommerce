using FooCommerce.Application.Enums.Membership;

namespace FooCommerce.Application.Dtos.Membership;

public record RoleModel
{
    public Guid Id { get; init; }
    public RoleTypes Type { get; init; }
}