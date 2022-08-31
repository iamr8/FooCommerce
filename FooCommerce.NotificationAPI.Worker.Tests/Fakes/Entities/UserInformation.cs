using FooCommerce.Domain;
using FooCommerce.NotificationAPI.Worker.Tests.Fakes.Enums;

namespace FooCommerce.NotificationAPI.Worker.Tests.Fakes.Entities;

public record UserInformation
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public UserInformationType Type { get; init; }
    public string Value { get; init; }
    public Guid UserId { get; init; }
}