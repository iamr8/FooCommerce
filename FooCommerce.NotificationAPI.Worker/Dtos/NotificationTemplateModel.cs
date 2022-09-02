using FooCommerce.Domain.Enums;

namespace FooCommerce.NotificationAPI.Worker.Dtos;

internal record NotificationTemplateModel
{
    public Guid Id { get; init; }
    public CommunicationType Type { get; init; }
    public string JsonTemplate { get; init; }
    public bool IncludeRequest { get; init; }
}