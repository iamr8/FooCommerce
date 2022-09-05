﻿using FooCommerce.NotificationAPI.Interfaces;
using FooCommerce.NotificationAPI.Models;
using FooCommerce.NotificationAPI.Models.Communications;

namespace FooCommerce.NotificationAPI.Worker.Contracts;

public interface QueueNotificationSms
    : INotificationId, INotificationOptionsBase
{
    NotificationReceiver Receiver { get; }
    NotificationSmsModel Model { get; }
}