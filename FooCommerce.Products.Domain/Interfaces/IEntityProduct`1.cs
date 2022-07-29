#nullable enable

namespace FooCommerce.Products.Domain.Interfaces
{
    public interface IEntityProduct<T> : IEntityProduct where T : IEntityProduct
    {
        T? Base { get; set; }
        ICollection<T> Extensions { get; set; }
    }
}