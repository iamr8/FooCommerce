﻿using FooCommerce.Application.Interfaces;
using FooCommerce.Application.Interfaces.Notifications;

namespace FooCommerce.Application.Models.Notifications.Options;

public record SendNotificationEmailOptions : INotificationSendToCommunicationOptions
{
    public IEndUser RequestInfo { get; set; }
    public INotificationOptions Options { get; init; }
    public INotificationModelFactory Factory { get; init; }
    public INotificationTemplate Template { get; init; }
    public string WebsiteUrl { get; init; }
    public bool IsImportant { get; init; }
}