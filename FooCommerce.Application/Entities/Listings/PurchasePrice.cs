﻿using FooCommerce.Domain.Interfaces.Database;

namespace FooCommerce.Application.Entities.Listings;

public record PurchasePrice
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public decimal Amount { get; init; }
    public Guid ListingId { get; init; }
}