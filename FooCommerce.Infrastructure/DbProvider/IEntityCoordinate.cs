using NetTopologySuite.Geometries;

namespace FooCommerce.Infrastructure.DbProvider;

public interface IEntityCoordinate
{
    Point Coordinate { get; init; }
}