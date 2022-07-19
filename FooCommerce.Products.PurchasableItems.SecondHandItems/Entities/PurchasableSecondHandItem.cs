using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Products.Interfaces;

namespace FooCommerce.Products.PurchasableItems.SecondHandItems.Entities;

public class PurchasableSecondHandItem : Entity, IProduct<PurchasableSecondHandItemAd>
{
    public long ExternalId { get; set; }
    public Guid? CategoryId { get; set; }
    public ICollection<PurchasableSecondHandItemAd> Ads { get; set; }
}