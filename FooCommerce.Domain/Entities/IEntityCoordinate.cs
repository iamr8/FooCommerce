
using NetTopologySuite.Geometries;

namespace FooCommerce.Domain.Entities;

public interface IEntityCoordinate
{
    Point Coordinate { get; init; }
}