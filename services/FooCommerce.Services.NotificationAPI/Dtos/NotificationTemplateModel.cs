using FooCommerce.Domain.Enums;

namespace FooCommerce.NotificationService.Dtos;

internal record NotificationTemplateModel
{
    public Guid Id { get; init; }
    public CommType Type { get; init; }
    public string JsonTemplate { get; init; }
    public bool IncludeRequest { get; init; }
}