﻿using FooCommerce.Domain.Enums;

namespace FooCommerce.NotificationAPI.Interfaces;

public interface INotificationTemplate
{
    Guid Id { get; }
    CommunicationType Communication { get; }
    IDictionary<string, string> Values { get; }
}