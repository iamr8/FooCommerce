using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Products.Interfaces;

namespace FooCommerce.Products.PurchasableItems.SecondHandItems.Entities;

public class PurchasableSecondHandItem : Entity, IEntityProduct<PurchasableSecondHandItemAd>
{
    public Guid? CategoryId { get; set; }
    public ICollection<PurchasableSecondHandItemAd> Ads { get; set; }
}