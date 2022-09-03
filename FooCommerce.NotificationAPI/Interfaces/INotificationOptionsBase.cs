﻿using FooCommerce.NotificationAPI.Enums;
using FooCommerce.NotificationAPI.Models;

namespace FooCommerce.NotificationAPI.Interfaces;

public interface INotificationOptionsBase : INotificationRequestInfo
{
    Guid? UserId { get; }
    NotificationAction Action { get; }
    IEnumerable<NotificationLink> Links { get; }
    IEnumerable<NotificationFormatter> Formatters { get; }
    IEnumerable<object> Bag { get; }
}