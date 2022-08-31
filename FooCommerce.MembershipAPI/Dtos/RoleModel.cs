using FooCommerce.MembershipAPI.Enums;

namespace FooCommerce.MembershipAPI.Dtos;

public record RoleModel
{
    public Guid Id { get; init; }
    public RoleType Type { get; init; }
}