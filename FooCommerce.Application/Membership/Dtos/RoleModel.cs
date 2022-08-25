using FooCommerce.Application.Membership.Enums;

namespace FooCommerce.Application.Membership.Dtos;

public record RoleModel
{
    public Guid Id { get; init; }
    public RoleType Type { get; init; }
}