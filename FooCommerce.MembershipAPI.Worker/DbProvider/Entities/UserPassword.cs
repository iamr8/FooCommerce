using FooCommerce.Domain;

namespace FooCommerce.MembershipAPI.Worker.DbProvider.Entities;

public record UserPassword
    : IEntity
{
    public UserPassword()
    {
    }

    public UserPassword(string hash, Guid userId)
    {
        Hash = hash;
        UserId = userId;
    }

    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public string Hash { get; init; }
    public Guid UserId { get; init; }
}