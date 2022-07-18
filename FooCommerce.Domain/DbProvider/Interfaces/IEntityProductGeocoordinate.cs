using NetTopologySuite.Geometries;

namespace FooCommerce.Domain.DbProvider.Interfaces
{
    public interface IEntityCoordinate
    {
        Point? Coordinate { get; set; }
    }
}