#nullable enable

using FooCommerce;

namespace FooCommerce.Products.Interfaces
{
    public interface IEntityProduct<T> : IEntityProduct where T : IEntityProduct
    {
        T? Base { get; set; }
        ICollection<T> Extensions { get; set; }
    }
}