using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.DbProvider.Interfaces;

namespace FooCommerce.Products.Domain.Ads.Entities
{
    public class AdFeature : Entity, IEntityProductFeature
    {
        public int Key { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? AdId { get; set; }
    }
}