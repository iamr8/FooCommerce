﻿using FooCommerce.Domain;

namespace FooCommerce.NotificationService.DbProvider.Entities;

public record Notification
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public short Action { get; init; } // NotificationAction
}