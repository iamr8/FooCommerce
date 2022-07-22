namespace FooCommerce.Products.Domain.Interfaces
{
    public interface IEntityProduct<T> : IEntityProduct where T : class
    {
        ICollection<T> Ads { get; set; }
    }
}