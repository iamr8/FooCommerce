using FooCommerce.Domain;
using FooCommerce.Domain.Enums;

namespace FooCommerce.Services.MembershipAPI.DbProvider.Entities;

public record UserCommunication
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public CommType Type { get; init; }
    public string Value { get; init; }
    public bool IsVerified { get; init; }
    public bool IsOpenId { get; init; }
    public byte? OpenIdProvider { get; init; }
    public string OpenIdScope { get; init; }
    public Guid UserId { get; init; }
    public virtual User User { get; init; }
}