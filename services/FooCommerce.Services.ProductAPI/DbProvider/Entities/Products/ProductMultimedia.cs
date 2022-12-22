using FooCommerce.Domain;
using NetTopologySuite.Geometries;

namespace FooCommerce.Services.ProductAPI.DbProvider.Entities.Products;

public record ProductMultimedia
    : IEntity, IEntityPublicId, IEntityMedia, IEntitySortable
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
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