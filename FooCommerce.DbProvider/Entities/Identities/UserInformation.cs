using FooCommerce.Domain;

namespace FooCommerce.DbProvider.Entities.Identities;

public record UserInformation
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public byte Type { get; init; } // UserInformationType
    public string Value { get; init; }
    public Guid UserId { get; init; }
}