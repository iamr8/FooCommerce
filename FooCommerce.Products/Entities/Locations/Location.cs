using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.DbProvider.Interfaces;
using FooCommerce.Products.Enums;

using NetTopologySuite.Geometries;

namespace FooCommerce.Products.Entities.Locations
{
    public class Location : Entity, IEntityCoordinate
    {
        // Country, State/Region/Province/Locality, City/County/Area, District, Quarter
        public LocationType Type { get; set; }

        public string Value { get; set; }
        public Point? Coordinate { get; set; }
        public Guid LocationId { get; set; }
    }
}