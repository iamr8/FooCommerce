﻿using FooCommerce.Application.Communications.Enums;
using FooCommerce.NotificationAPI.Interfaces;

namespace FooCommerce.NotificationAPI.Worker.Events;

public interface NotificationSent
    : INotificationId
{
    CommunicationType Gateway { get; }
}