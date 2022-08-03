using FooCommerce.Domain.DbProvider;

namespace FooCommerce.Products.Products.Entities
{
    public record Category : Entity
    {
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; }
    }
}