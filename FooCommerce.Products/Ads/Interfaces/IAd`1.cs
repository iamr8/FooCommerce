using FooCommerce.Products.Products.Interfaces;

namespace FooCommerce.Products.Ads.Interfaces
{
    public interface IAd<TProduct> : IAd where TProduct : IProduct
    {
        TProduct Product { get; set; }
    }
}