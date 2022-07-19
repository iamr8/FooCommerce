using FooCommerce.Products.Products.Interfaces;

namespace FooCommerce.Products.Ads.Interfaces
{
    public interface IEntityProductAd<TProduct> : IEntityAd where TProduct : IEntityProduct
    {
        TProduct Product { get; set; }
    }
}