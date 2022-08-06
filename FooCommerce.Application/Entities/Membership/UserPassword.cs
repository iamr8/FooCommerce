﻿using FooCommerce.Domain.Entities;

namespace FooCommerce.Application.Entities.Membership;

public record UserPassword
    : IEntity
{
    public UserPassword()
    {
    }

    public UserPassword(string hash, Guid userId)
    {
        Hash = hash;
        UserId = userId;
    }

    public Guid Id { get; init; }
    public DateTimeOffset Created { get; init; }
    public byte[] RowVersion { get; init; }
    public string Hash { get; init; }
    public Guid UserId { get; init; }
}