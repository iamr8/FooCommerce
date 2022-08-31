﻿using FooCommerce.NotificationAPI.Worker.Enums;

namespace FooCommerce.NotificationAPI.Worker.Contracts;

public interface UpdateUserNotificationState
{
    Guid UserNotificationId { get; }
    UserNotificationState State { get; }
}