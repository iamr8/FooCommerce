using FooCommerce.Application.DbProvider;

namespace FooCommerce.Products.Entities
{
    public record AdLike : Entity
    {
        public Guid AdId { get; set; }
    }
}