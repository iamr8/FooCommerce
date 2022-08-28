﻿using FooCommerce.Application.Membership.Enums;

namespace FooCommerce.NotificationAPI.Dtos;

internal record NotificationTemplateModel
{
    public Guid Id { get; init; }
    public CommunicationType Type { get; init; }
    public string JsonTemplate { get; init; }
    public bool IncludeRequest { get; init; }
}