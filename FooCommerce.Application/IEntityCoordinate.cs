using NetTopologySuite.Geometries;

namespace FooCommerce.Application;

public interface IEntityCoordinate
{
    Point Coordinate { get; init; }
}