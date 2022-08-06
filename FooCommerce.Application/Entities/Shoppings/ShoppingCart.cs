﻿using FooCommerce.Domain.Entities;

namespace FooCommerce.Application.Entities.Shoppings;

public record ShoppingCart
    : IEntity, IEntitySoftDeletable, IEntityPublicId
{
    public Guid Id { get; init; }
    public DateTimeOffset Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; init; }
    public uint PublicId { get; init; }
    public ushort Quantity { get; init; }
    public decimal Amount { get; init; }
    public Guid PurchasePriceId { get; init; }
}