﻿using FooCommerce.Application.Enums.Membership;

namespace FooCommerce.Application.Dtos.Listings;

public record LocationModel
{
    public Guid Id { get; init; }
    public LocationDivisions Division { get; init; }

    public string Name { get; init; }
    public uint PublicId { get; init; }
    public Guid? ParentId { get; init; }
}