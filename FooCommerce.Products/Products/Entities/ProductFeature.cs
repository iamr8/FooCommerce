using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.DbProvider.Interfaces;

namespace FooCommerce.Products.Products.Entities
{
    public class ProductFeature : Entity, IEntityProductFeature
    {
        public int Key { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? AdId { get; set; }
    }
}