using FooCommerce.Application.DbProvider;
using FooCommerce.Application.DbProvider.Interfaces;

namespace FooCommerce.Products.Entities
{
    public record Feature : Entity, IEntityPublicId
    {
        public long PublicId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<AdFeature> Children { get; set; }
    }
}