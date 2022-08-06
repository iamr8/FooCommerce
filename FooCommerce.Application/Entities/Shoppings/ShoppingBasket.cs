﻿using FooCommerce.Domain.Entities;

namespace FooCommerce.Application.Entities.Shoppings;

public record ShoppingBasket
    : IEntity, IEntitySoftDeletable, IEntityPublicId
{
    public Guid Id { get; init; }
    public DateTimeOffset Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; init; }
    public uint PublicId { get; init; }
    public Guid TopCategoryId { get; init; }
    public Guid UserSubscriptionId { get; init; }
}