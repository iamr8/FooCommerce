namespace FooCommerce.Products.Products.Interfaces
{
    public interface IProduct<T> : IProduct where T : class
    {
        ICollection<T> Ads { get; set; }
    }
}