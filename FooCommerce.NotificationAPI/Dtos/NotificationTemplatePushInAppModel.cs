﻿using FooCommerce.Application.Localization.Models;
using FooCommerce.Application.Membership.Enums;
using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.NotificationAPI.Dtos;

public record NotificationTemplatePushInAppModel : INotificationTemplate
{
    internal NotificationTemplatePushInAppModel(Guid id)
    {
        this.Id = id;
    }
    public Guid Id { get; }
    public CommunicationType Communication => CommunicationType.Push_InApp;
    public LocalizerValueCollection Subject { get; init; }
    public LocalizerValueCollection Message { get; init; }
    public IDictionary<string, string> Values { get; }
}