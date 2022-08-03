using FooCommerce.Products.Domain.Entities;

namespace FooCommerce.Products.Domain.Interfaces
{
    public interface IEntityProductLocation
    {
        ICollection<AdLocation> Locations { get; set; }
    }
}