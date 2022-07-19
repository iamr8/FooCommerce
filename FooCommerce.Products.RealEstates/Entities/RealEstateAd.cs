using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Ads.Interfaces;

namespace FooCommerce.Products.RealEstates.Entities;

public class RealEstateAd : Entity, IAd<RealEstate>
{
    public DateTime EndDate { get; set; }
    public Guid ProductId { get; set; }
    public Guid? ParentAdId { get; set; }
    public Guid UserSubscriptionId { get; set; }
    public RealEstate Product { get; set; }
}