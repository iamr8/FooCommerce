using System.Net;

using FooCommerce.Application.DbProvider;
using FooCommerce.Application.DbProvider.Interfaces;

namespace FooCommerce.Products.Entities
{
    public record AdSave : Entity, IEntityRequestLog
    {
        public IPAddress IPAddress { get; set; }
        public string UserAgent { get; set; }
        public Guid AdId { get; set; }
    }
}