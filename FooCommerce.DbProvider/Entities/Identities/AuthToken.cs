using FooCommerce.Domain;

namespace FooCommerce.DbProvider.Entities.Identities;

public record AuthToken
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public byte Action { get; init; } // AuthTokenAction
    public string Token { get; init; }
    public DateTimeOffset? Authorized { get; set; }
    public string More { get; init; }
    public Guid UserCommunicationId { get; init; }
}