using System.Net;

using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.DbProvider.Interfaces;

namespace FooCommerce.Products.Domain.Entities
{
    public record AdSave : Entity, IEntityRequestLog
    {
        public IPAddress IPAddress { get; set; }
        public string UserAgent { get; set; }
        public Guid AdId { get; set; }
    }
}