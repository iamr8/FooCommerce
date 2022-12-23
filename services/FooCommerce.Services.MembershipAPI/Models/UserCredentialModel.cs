using FooCommerce.MembershipService.Enums;

namespace FooCommerce.MembershipService.Models;

public record UserCredentialModel
{
    public Guid UserId { get; init; }
    public string Hash { get; init; }
    public Guid CommunicationId { get; init; }
    public Guid RoleId { get; init; }
    public RoleType RoleType { get; init; }
}