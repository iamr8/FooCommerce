using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.DbProvider.Interfaces;
using FooCommerce.Products.Interfaces;

using NetTopologySuite.Geometries;

namespace FooCommerce.Products.Entities
{
    public record AdVideo : Entity, IEntityCoordinate, IEntityPublicId, IEntitySortable, IEntityMedia
    {
        public long PublicId { get; set; }
        public string Path { get; set; }
        public bool IsOriginal { get; set; }
        public int Order { get; set; }
        public Point Coordinate { get; set; }
        public Guid AdId { get; set; }
    }
}