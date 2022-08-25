using FooCommerce.Application.Membership.Enums;

namespace FooCommerce.Application.Membership.Models;

public record UserCommunicationModel
{
    public Guid Id { get; init; }
    public CommunicationType Type { get; init; }
    public string Value { get; init; }
}