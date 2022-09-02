﻿using FooCommerce.NotificationAPI.Interfaces;
using FooCommerce.NotificationAPI.Models;
using FooCommerce.NotificationAPI.Models.Communications;

namespace FooCommerce.NotificationAPI.Contracts;

public interface QueueNotificationPush
    : INotificationId, INotificationOptionsBase
{
    NotificationReceiver Receiver { get; }
    NotificationPushModel Model { get; }
}