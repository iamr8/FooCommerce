using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Domain.Interfaces;
using FooCommerce.Products.Locations.Enums;

using NetTopologySuite.Geometries;

namespace FooCommerce.Products.Locations.Entities
{
    public record Location : Entity, IEntityCoordinate
    {
        // Country, State/Region/Province/Locality, City/County/Area, District, Quarter
        public LocationType Type { get; set; }

        public string Value { get; set; }
        public Point? Coordinate { get; set; }
        public Guid LocationId { get; set; }
    }
}