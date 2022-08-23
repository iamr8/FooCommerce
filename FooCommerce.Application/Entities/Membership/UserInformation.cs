﻿using FooCommerce.Application.Enums.Membership;
using FooCommerce.Domain.Interfaces.Database;

namespace FooCommerce.Application.Entities.Membership;

public record UserInformation
    : IEntity
{
    public UserInformation()
    {
    }

    public UserInformation(UserInformationType type, string value, Guid userId)
    {
        Type = type;
        Value = value;
        UserId = userId;
    }

    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public UserInformationType Type { get; init; }
    public string Value { get; init; }
    public Guid UserId { get; init; }
}