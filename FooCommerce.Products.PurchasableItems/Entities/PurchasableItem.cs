using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Domain.Interfaces;

namespace FooCommerce.Products.PurchasableItems.Entities;

public class PurchasableItem : Entity, IEntityProduct<PurchasableItemAd>
{
    public Guid? CategoryId { get; set; }
    public ICollection<PurchasableItemAd> Ads { get; set; }
}