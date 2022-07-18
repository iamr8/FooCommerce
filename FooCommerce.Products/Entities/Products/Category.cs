using FooCommerce.Domain.DbProvider;

namespace FooCommerce.Products.Entities.Products
{
    public class Category : Entity
    {
        public Guid? ParentId { get; set; }
    }
}