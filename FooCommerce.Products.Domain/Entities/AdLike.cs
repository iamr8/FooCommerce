using FooCommerce.Domain.DbProvider;

namespace FooCommerce.Products.Domain.Entities
{
    public record AdLike : Entity
    {
        public Guid AdId { get; set; }
    }
}