using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.Entities;
using FooCommerce.Products.Entities;
using FooCommerce.Products.Interfaces;

using NetTopologySuite.Geometries;

namespace FooCommerce.Products.Vehicles.Entities;

public record Vehicle : Entity, IEntityProduct<Vehicle>, IEntityBarcode, IEntityProductLocation
{
    public Point? Coordinate { get; set; }
    public string? Barcode { get; set; }
    public uint PublicId { get; init; }
    public Guid? CategoryId { get; set; }
    public DateTime EndDate { get; set; }
    public Guid? BaseId { get; set; }
    public Guid UserSubscriptionId { get; set; }
    public ICollection<AdFeature> Features { get; set; }
    public ICollection<AdSpecification> Specifications { get; set; }
    public ICollection<AdView> Views { get; set; }
    public ICollection<AdImage> Images { get; set; }
    public ICollection<AdVideo> Videos { get; set; }
    public ICollection<AdSave> Saves { get; set; }
    public ICollection<AdLike> Likes { get; set; }
    public ICollection<AdComment> Comments { get; set; }
    public ICollection<AdLocation> Locations { get; set; }

    public Vehicle? Base { get; set; }
    public ICollection<Vehicle> Extensions { get; set; }
}