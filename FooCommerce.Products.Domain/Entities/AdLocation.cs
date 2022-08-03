using FooCommerce.Domain.DbProvider;
using FooCommerce.Products.Domain.Enums;

namespace FooCommerce.Products.Domain.Entities
{
    public record AdLocation : Entity
    {
        // Country, State/Region/Province/Locality, City/County/Area, District, Quarter?, Street, PostalCode
        public AdLocationType Type { get; set; }

        public string Value { get; set; }
        public Guid? LocationId { get; set; }
        public Guid AdId { get; set; }
    }
}