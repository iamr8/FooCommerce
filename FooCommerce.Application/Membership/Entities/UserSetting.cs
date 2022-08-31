﻿using FooCommerce.Domain;

namespace FooCommerce.Application.Membership.Entities;

public record UserSetting
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public string Key { get; init; }
    public string Value { get; init; }
    public Guid UserId { get; init; }
}