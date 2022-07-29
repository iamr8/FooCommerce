using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.DbProvider.Interfaces;
using FooCommerce.Products.Domain.Ads.Entities;
using FooCommerce.Products.Domain.Interfaces;

using NetTopologySuite.Geometries;

namespace FooCommerce.Products.Vehicles.Entities;

public class Vehicle : Entity, IEntityProduct<Vehicle>, IEntityBarcode
{
    public Point? Coordinate { get; set; }
    public string? Barcode { get; set; }
    public long ExternalId { get; set; }
    public Guid? CategoryId { get; set; }
    public DateTime EndDate { get; set; }
    public Guid? BaseId { get; set; }
    public Guid UserSubscriptionId { get; set; }
    public ICollection<AdFeature> Features { get; set; }
    public ICollection<AdSpecification> Specifications { get; set; }
    public ICollection<AdView> Views { get; set; }
    public ICollection<AdImage> Images { get; set; }
    public ICollection<AdWish> Wishes { get; set; }
    public ICollection<AdLocation> Locations { get; set; }

    public Vehicle? Base { get; set; }
    public ICollection<Vehicle> Extensions { get; set; }
}