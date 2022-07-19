using FooCommerce.Domain.DbProvider;

namespace FooCommerce.Products.Products.Entities
{
    public class Category : Entity
    {
        public Guid? ParentId { get; set; }
    }
}