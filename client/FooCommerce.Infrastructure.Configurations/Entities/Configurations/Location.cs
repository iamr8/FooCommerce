#nullable enable

using FooCommerce.Domain;
using NetTopologySuite.Geometries;

namespace FooCommerce.Infrastructure.Configurations.Entities.Configurations;

public record Location
    : IEntity, IEntitySoftDeletable, IEntityVisibility, IEntityExternalId
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public byte[] RowVersion { get; init; }
    public bool IsDeleted { get; set; }
    public uint ExternalId { get; init; }
    public bool IsInvisible { get; set; }

    // Country, State/Region/Province/Locality, City/County/Area, District, Quarter
    public byte Division { get; init; } // LocationDivision
    public string Name { get; init; }
    public Point? Coordinate { get; init; }
    public Guid? ParentId { get; init; }
}