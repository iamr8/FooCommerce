using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Ads.Interfaces;

namespace FooCommerce.Products.Vehicles.Entities;

public class VehicleAd : Entity, IAd<Vehicle>
{
    public DateTime EndDate { get; set; }
    public Guid ProductId { get; set; }
    public Guid? ParentAdId { get; set; }
    public Guid UserSubscriptionId { get; set; }
    public Vehicle Product { get; set; }
}