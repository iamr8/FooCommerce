
using NetTopologySuite.Geometries;

namespace FooCommerce.Products.Interfaces
{
    public interface IEntityCoordinate
    {
        Point Coordinate { get; set; }
    }
}