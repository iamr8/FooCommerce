using NetTopologySuite.Geometries;

namespace FooCommerce.Application.Interfaces;

public interface IEntityCoordinate
{
    Point Coordinate { get; init; }
}