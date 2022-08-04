using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.DbProvider.Interfaces;

namespace FooCommerce.Products.Entities
{
    public record Specification : Entity, IEntityPublicId
    {
        public long PublicId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<AdSpecification> Children { get; set; }
    }
}