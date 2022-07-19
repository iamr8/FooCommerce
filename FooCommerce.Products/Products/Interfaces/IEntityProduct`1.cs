namespace FooCommerce.Products.Products.Interfaces
{
    public interface IEntityProduct<T> : IEntityProduct where T : class
    {
        ICollection<T> Ads { get; set; }
    }
}