using FooCommerce.Domain.DbProvider.Interfaces;

namespace FooCommerce.Products.Domain.Interfaces
{
    public interface IEntityProductAd : IEntityExternalId
    {
        DateTime EndDate { get; set; }
        Guid ProductId { get; set; }
        Guid? ParentAdId { get; set; }
        Guid UserSubscriptionId { get; set; }
    }
}