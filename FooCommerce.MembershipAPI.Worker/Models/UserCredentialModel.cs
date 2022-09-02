using FooCommerce.MembershipAPI.Enums;

namespace FooCommerce.MembershipAPI.Worker.Models;

public record UserCredentialModel
{
    public Guid UserId { get; init; }
    public string Hash { get; init; }
    public Guid CommunicationId { get; init; }
    public Guid RoleId { get; init; }
    public RoleType RoleType { get; init; }
}