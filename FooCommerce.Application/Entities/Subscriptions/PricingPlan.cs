﻿using FooCommerce.Domain.Entities;

namespace FooCommerce.Application.Entities.Subscriptions;

public record PricingPlan
    : IEntity, IEntitySortable
{
    public Guid Id { get; init; }
    public DateTimeOffset Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsActive { get; init; }
    public ushort Type { get; init; }
    public decimal Price { get; init; }
    public string AcceptedRoles { get; init; }
    public int Order { get; set; }
}