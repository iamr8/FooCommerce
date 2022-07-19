using FooCommerce.Domain.DbProvider.Interfaces;

namespace FooCommerce.Products.Products.Interfaces
{
    public interface IProduct : IEntityExternalId
    {
        public Guid? CategoryId { get; set; }
    }
}