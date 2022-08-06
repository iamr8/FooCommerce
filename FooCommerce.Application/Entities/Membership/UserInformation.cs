using FooCommerce.Application.Enums.Membership;
using FooCommerce.Domain.Entities;

namespace FooCommerce.Application.Entities.Membership;

public record UserInformation
    : IEntity
{
    public UserInformation()
    {
    }

    public UserInformation(UserInformationTypes type, string value, Guid userId)
    {
        Type = type;
        Value = value;
        UserId = userId;
    }

    public Guid Id { get; init; }
    public DateTimeOffset Created { get; init; }
    public byte[] RowVersion { get; init; }
    public UserInformationTypes Type { get; init; }
    public string Value { get; init; }
    public Guid UserId { get; init; }
}