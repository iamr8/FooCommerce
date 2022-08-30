﻿using FooCommerce.Domain.Interfaces.Database;
using FooCommerce.NotificationAPI.Bridge.Enums;

namespace FooCommerce.NotificationAPI.Entities;

public record Notification
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public NotificationAction Action { get; init; }
}