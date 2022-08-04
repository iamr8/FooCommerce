using FooCommerce.Products.Entities;

namespace FooCommerce.Products.Interfaces
{
    public interface IEntityProductLocation
    {
        ICollection<AdLocation> Locations { get; set; }
    }
}