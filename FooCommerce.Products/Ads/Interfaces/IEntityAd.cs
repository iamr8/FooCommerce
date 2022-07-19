using FooCommerce.Domain.DbProvider.Interfaces;

namespace FooCommerce.Products.Ads.Interfaces
{
    public interface IEntityAd : IEntityExternalId
    {
        DateTime EndDate { get; set; }
        Guid ProductId { get; set; }
        Guid? ParentAdId { get; set; }
        Guid UserSubscriptionId { get; set; }
    }
}