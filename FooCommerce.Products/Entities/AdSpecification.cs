using FooCommerce.Application.DbProvider;
using FooCommerce.Products.Interfaces;

namespace FooCommerce.Products.Entities
{
    public record AdSpecification : Entity, IEntityProductSpecification
    {
        public string Value { get; set; }
        public Guid? AdId { get; set; }
        public Guid? ParentId { get; set; }
        public virtual Specification Parent { get; set; }
    }
}