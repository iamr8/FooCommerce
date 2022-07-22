namespace FooCommerce.Products.Domain.Interfaces
{
    public interface IEntityProductAd<TProduct> : IEntityProductAd where TProduct : IEntityProduct
    {
        TProduct Product { get; set; }
    }
}