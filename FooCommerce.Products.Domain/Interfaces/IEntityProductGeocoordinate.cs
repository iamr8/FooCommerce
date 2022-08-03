
using NetTopologySuite.Geometries;

namespace FooCommerce.Products.Domain.Interfaces
{
    public interface IEntityCoordinate
    {
        Point Coordinate { get; set; }
    }
}