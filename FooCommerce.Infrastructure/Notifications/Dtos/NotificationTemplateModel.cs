using FooCommerce.Application.Enums.Membership;

namespace FooCommerce.Infrastructure.Notifications.Dtos;

internal record NotificationTemplateModel
{
    public Guid Id { get; init; }
    public CommunicationType Type { get; init; }
    public string JsonTemplate { get; init; }
    public bool IncludeRequest { get; init; }
}