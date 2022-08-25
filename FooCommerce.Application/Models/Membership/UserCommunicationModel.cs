using FooCommerce.Application.Enums.Membership;

namespace FooCommerce.Application.Models.Membership;

public record UserCommunicationModel
{
    public Guid Id { get; init; }
    public CommunicationType Type { get; init; }
    public string Value { get; init; }
}