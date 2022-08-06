using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.Entities;

namespace FooCommerce.Products.Entities
{
    public record Specification : Entity, IEntityPublicId
    {
        public uint PublicId { get; init; }
        public string Name { get; set; }
        public virtual ICollection<AdSpecification> Children { get; set; }
    }
}