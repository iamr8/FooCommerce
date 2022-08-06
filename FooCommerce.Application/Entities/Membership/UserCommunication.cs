using FooCommerce.Application.Enums.Membership;
using FooCommerce.Domain.Entities;

namespace FooCommerce.Application.Entities.Membership;

public record UserCommunication
    : IEntity
{
    public UserCommunication()
    {
    }

    public UserCommunication(UserCommunicationTypes type, string value, Guid userId) : this()
    {
        Type = type;
        Value = value;
        UserId = userId;
    }

    public Guid Id { get; init; }
    public DateTimeOffset Created { get; init; }
    public byte[] RowVersion { get; init; }
    public UserCommunicationTypes Type { get; init; }
    public string Value { get; init; }
    public bool IsVerified { get; init; }
    public bool IsOpenId { get; init; }
    public ushort? OpenIdProvider { get; init; }
    public string? OpenIdScope { get; init; }
    public Guid UserId { get; init; }
}