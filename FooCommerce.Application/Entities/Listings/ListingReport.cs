﻿#nullable enable

using FooCommerce;
using FooCommerce.Domain.Entities;

namespace FooCommerce.Application.Entities.Listings;

public record ListingReport
    : IEntity
{
    public Guid Id { get; init; }
    public DateTimeOffset Created { get; init; }
    public byte[] RowVersion { get; init; }
    public ushort Reason { get; init; }
    public string? Description { get; init; }
    public Guid ListingId { get; init; }
    public Guid UserId { get; init; }
}