using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.DbProvider.Interfaces;
using FooCommerce.Products.Ads.Interfaces;

namespace FooCommerce.Products.Vehicles.Entities;

public class VehicleAd : Entity, IEntityProductAd<Vehicle>, IEntityBarcode
{
    public long ExternalId { get; set; }
    public string? Barcode { get; set; }
    public DateTime EndDate { get; set; }
    public Guid ProductId { get; set; }
    public Guid? ParentAdId { get; set; }
    public Guid UserSubscriptionId { get; set; }
    public Vehicle Product { get; set; }
}