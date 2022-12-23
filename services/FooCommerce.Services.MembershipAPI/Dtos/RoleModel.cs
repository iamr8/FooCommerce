using FooCommerce.MembershipService.Enums;

namespace FooCommerce.MembershipService.Dtos;

public record RoleModel
{
    public Guid Id { get; init; }
    public RoleType Type { get; init; }
}