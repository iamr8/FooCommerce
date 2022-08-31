﻿using FooCommerce.Domain;

namespace FooCommerce.Application.Listings.Entities;

public record PurchasePrice
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public decimal Amount { get; init; }
    public Guid ListingId { get; init; }
}