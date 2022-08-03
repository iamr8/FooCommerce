using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Domain.Interfaces;

namespace FooCommerce.Products.Domain.Entities
{
    public record AdSpecification : Entity, IEntityProductSpecification
    {
        public string Value { get; set; }
        public Guid? AdId { get; set; }
        public Guid? ParentId { get; set; }
        public virtual Specification Parent { get; set; }
    }
}