﻿using FooCommerce.Application.Enums.Membership;
using FooCommerce.Domain.Interfaces.Database;

namespace FooCommerce.Application.Entities.Membership;

public record UserCommunication
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public CommunicationType Type { get; init; }
    public string Value { get; init; }
    public bool IsVerified { get; init; }
    public bool IsOpenId { get; init; }
    public byte? OpenIdProvider { get; init; }
    public string? OpenIdScope { get; init; }
    public Guid UserId { get; init; }
}