using FooCommerce.Application.DbProvider;
using FooCommerce.Products.Enums;

namespace FooCommerce.Products.Entities
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