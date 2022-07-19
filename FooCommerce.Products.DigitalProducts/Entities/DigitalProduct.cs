using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Products.Interfaces;

namespace FooCommerce.Products.DigitalProducts.Entities;

public class DigitalProduct : Entity, IProduct<DigitalProductAd>
{
    public string Name { get; set; }
    public long ExternalId { get; set; }
    public Guid? CategoryId { get; set; }
    public virtual ICollection<DigitalProductAd> Ads { get; set; }
}