using FooCommerce.Application.Membership.Enums;

namespace FooCommerce.NotificationAPI.Dtos;

internal record NotificationTemplateModel(Guid Id, CommunicationType Type, string JsonTemplate, bool IncludeRequest);