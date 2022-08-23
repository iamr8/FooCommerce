﻿using System.Globalization;
using System.Net;

using FooCommerce.Application.Interfaces.Notifications;
using FooCommerce.Domain.Interfaces;

namespace FooCommerce.Application.Models.Notifications.Options;

public record NotificationEmailModelOptions : INotificationCommunicationOptions
{
    public IPAddress? IPAddress { get; init; }
    public DateTime LocalDateTime { get; set; }
    public string WebsiteUrl { get; set; }
    public RegionInfo? Country { get; init; }
    public ILocalizer Localizer { get; init; }
    public string Platform { get; init; } // $"{detectionService.Platform.Name} {detectionService.Platform.Version}"
    public string Browser { get; init; } // $"{detectionService.Browser.Name} {detectionService.Browser.Version}"
}