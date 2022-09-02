using FooCommerce.Infrastructure.Locations.Enums;

namespace FooCommerce.Infrastructure.Listings.Dtos;

public record LocationModel
{
    public Guid Id { get; init; }
    public LocationDivision Division { get; init; }

    public string Name { get; init; }
    public uint PublicId { get; init; }
    public Guid? ParentId { get; init; }
}