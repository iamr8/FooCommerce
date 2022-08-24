using FooCommerce.Application.Enums.Membership;

namespace FooCommerce.NotificationAPI.Dtos;

internal record NotificationTemplateModel(Guid Id, CommunicationType Type, string JsonTemplate, bool IncludeRequest);