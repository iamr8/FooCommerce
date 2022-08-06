using FooCommerce.Domain.Entities;

using NetTopologySuite.Geometries;

namespace FooCommerce.Application.Entities.Products;

public record ProductMultimedia
    : IEntity, IEntityPublicId, IEntityCoordinate, IEntityMedia, IEntitySortable
{
    public Guid Id { get; init; }
    public DateTimeOffset Created { get; init; }
    public byte[] RowVersion { get; init; }
    public uint PublicId { get; init; }
    public bool IsVideo { get; init; }
    public Point Coordinate { get; init; }
    public string Metadata { get; init; }
    public string Path { get; init; }
    public bool IsOriginal { get; init; }
    public int Order { get; set; }
    public Guid ProductId { get; init; }
}