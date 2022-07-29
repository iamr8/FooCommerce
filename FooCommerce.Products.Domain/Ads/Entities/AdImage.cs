using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.DbProvider.Interfaces;

using NetTopologySuite.Geometries;

namespace FooCommerce.Products.Domain.Ads.Entities
{
    public class AdImage : Entity, IEntityCoordinate, IEntityExternalId, IEntitySortable, IEntityImage
    {
        public long ExternalId { get; set; }
        public string Path { get; set; }
        public bool IsOriginal { get; set; }
        public int Order { get; set; }
        public Point Coordinate { get; set; }
        public Guid AdId { get; set; }
    }
}