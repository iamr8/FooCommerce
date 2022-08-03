using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Domain.Interfaces;

namespace FooCommerce.Products.Domain.Entities
{
    public record AdFeature : Entity, IEntityProductFeature
    {
        public Guid? ProductId { get; set; }
        public Guid? AdId { get; set; }
        public Guid? ParentId { get; set; }
        public virtual Feature Parent { get; set; }
    }
}