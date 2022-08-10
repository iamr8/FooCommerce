using FooCommerce.Application.Enums.Membership;

namespace FooCommerce.Infrastructure.Membership.Dtos;

public record RoleModel
{
    public Guid Id { get; init; }
    public RoleTypes Type { get; init; }
}