using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.Entities;

namespace FooCommerce.Products.Entities
{
    public record Feature : Entity, IEntityPublicId
    {
        public uint PublicId { get; init; }
        public string Name { get; set; }
        public virtual ICollection<AdFeature> Children { get; set; }
    }
}