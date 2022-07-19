using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.DbProvider.Interfaces;
using FooCommerce.Products.Products.Interfaces;

using NetTopologySuite.Geometries;

namespace FooCommerce.Products.RealEstates.Entities;

public class RealEstate : Entity, IProduct<RealEstateAd>, IEntityCoordinate, IEntityProductBarcode
{
    public long ExternalId { get; set; }
    public string Barcode { get; set; }
    public Point? Coordinate { get; set; }
    public Guid? CategoryId { get; set; }
    public ICollection<RealEstateAd> Ads { get; set; }
}