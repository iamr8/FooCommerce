using FooCommerce.Domain;
using FooCommerce.Domain.Enums;

namespace FooCommerce.Services.NotificationAPI.DbProvider.Entities;

public record NotificationTemplate
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public CommType Type { get; init; }

    public string JsonTemplate { get; init; }
    public bool IncludeRequest { get; init; }
    public Guid NotificationId { get; init; }
}