using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.Entities;

namespace FooCommerce.Products.Products.Entities
{
    public record AdCategory : Entity, IEntityPublicId
    {
        public uint PublicId { get; init; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public virtual AdCategory Parent { get; set; }
        public virtual ICollection<AdCategory> Children { get; set; }
    }
}