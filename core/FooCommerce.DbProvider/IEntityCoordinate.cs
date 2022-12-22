using NetTopologySuite.Geometries;

namespace FooCommerce.DbProvider;

public interface IEntityCoordinate
{
    Point Coordinate { get; init; }
}