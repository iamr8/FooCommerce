﻿using FooCommerce.Domain;

namespace FooCommerce.Infrastructure.Configurations.Entities.Configurations;

public record Translation
    : IEntity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public string Key { get; init; }
    public string Value { get; init; }
}