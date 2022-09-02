using FooCommerce.Domain;
using FooCommerce.Infrastructure.DbProvider;
using FooCommerce.Infrastructure.Locations.Enums;

using NetTopologySuite.Geometries;

namespace FooCommerce.Infrastructure.Listings.Entities;

public record Location
    : IEntity, IEntitySoftDeletable, IEntityHideable, IEntityPublicId, IEntityCoordinate
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; init; }
    public uint PublicId { get; init; }
    public bool IsHidden { get; init; }

    // Country, State/Region/Province/Locality, City/County/Area, District, Quarter
    public LocationDivision Division { get; init; }
    public string Name { get; init; }
    public Point Coordinate { get; init; }
    public Guid? ParentId { get; init; }
}