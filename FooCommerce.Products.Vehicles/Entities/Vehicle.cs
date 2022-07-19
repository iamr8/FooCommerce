using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.DbProvider.Interfaces;
using FooCommerce.Products.Products.Interfaces;

using NetTopologySuite.Geometries;

namespace FooCommerce.Products.Vehicles.Entities;

public class Vehicle : Entity, IProduct<VehicleAd>, IEntityCoordinate, IEntityProductBarcode
{
    public long ExternalId { get; set; }
    public string Barcode { get; set; }
    public Point? Coordinate { get; set; }
    public Guid? CategoryId { get; set; }
    public ICollection<VehicleAd> Ads { get; set; }
}