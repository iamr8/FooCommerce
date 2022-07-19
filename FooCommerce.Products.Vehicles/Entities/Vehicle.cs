using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.DbProvider.Interfaces;
using FooCommerce.Products.Products.Interfaces;

using NetTopologySuite.Geometries;

namespace FooCommerce.Products.Vehicles.Entities;

public class Vehicle : Entity, IEntityProduct<VehicleAd>, IEntityCoordinate
{
    public Point? Coordinate { get; set; }
    public Guid? CategoryId { get; set; }
    public ICollection<VehicleAd> Ads { get; set; }
}