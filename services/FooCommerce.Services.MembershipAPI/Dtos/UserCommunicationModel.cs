using FooCommerce.Domain.Enums;

namespace FooCommerce.MembershipService.Dtos;

public record UserCommunicationModel
{
    public UserCommunicationModel()
    {
    }

    public UserCommunicationModel(Guid id, CommType type, string value, bool isVerified)
    {
        Id = id;
        Type = type;
        Value = value;
        IsVerified = isVerified;
    }

    public Guid Id { get; init; }
    public CommType Type { get; init; }
    public string Value { get; init; }
    public bool IsVerified { get; init; }
}