using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Products.Interfaces;

namespace FooCommerce.Products.DigitalProducts.Entities;

public class DigitalProduct : Entity, IEntityProduct<DigitalProductAd>
{
    public string Name { get; set; }
    public Guid? CategoryId { get; set; }
    public virtual ICollection<DigitalProductAd> Ads { get; set; }
}