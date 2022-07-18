using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.DbProvider.Interfaces;

using NetTopologySuite.Geometries;

namespace FooCommerce.Products.Entities.Products
{
    public class Product : Entity, IEntityExternalId, IEntityProductBarcode, IEntityCoordinate
    {
        public long ExternalId { get; set; }
        public Point? Coordinate { get; set; }
        public string Barcode { get; set; }
        public int BarcodeType { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid AdId { get; set; }
    }
}