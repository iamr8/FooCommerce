﻿using FooCommerce.Domain;

namespace FooCommerce.PaymentService.DbProvider.Entities;

public record Checkout
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public int OrderId { get; init; }
    public ushort Status { get; init; }
    public decimal Amount { get; init; }
    public bool IsSuccessful { get; init; }
    public Guid BasketId { get; init; }
}