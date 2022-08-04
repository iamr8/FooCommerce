using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.DbProvider.Interfaces;

namespace FooCommerce.Products.Products.Entities
{
    public record AdCategory : Entity, IEntityPublicId
    {
        public long PublicId { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public virtual AdCategory Parent { get; set; }
        public virtual ICollection<AdCategory> Children { get; set; }
    }
}