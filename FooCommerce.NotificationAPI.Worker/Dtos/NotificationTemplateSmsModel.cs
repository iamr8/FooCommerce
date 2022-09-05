﻿using FooCommerce.Domain.Enums;
using FooCommerce.Localization.Models;
using FooCommerce.NotificationAPI.Worker.Interfaces;

namespace FooCommerce.NotificationAPI.Worker.Dtos;

public record NotificationTemplateSmsModel : INotificationTemplate
{
    internal NotificationTemplateSmsModel(Guid id)
    {
        this.Id = id;
    }
    public Guid Id { get; }
    public CommunicationType Communication => CommunicationType.Mobile_Sms;
    public LocalizerValueCollection Text { get; init; }
    public IDictionary<string, string> Values { get; }
}