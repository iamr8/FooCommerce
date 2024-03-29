﻿using FooCommerce.Domain.Enums;

namespace FooCommerce.NotificationService.Exceptions;

public class ReceiverNotFoundException : Exception
{
    public ReceiverNotFoundException(CommType type) : base($"Action {type} needs to send notification via {type}, but unable to find receiver's address.")
    {
    }
}