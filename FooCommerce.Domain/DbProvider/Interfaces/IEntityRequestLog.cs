using System.Net;

namespace FooCommerce.Domain.DbProvider.Interfaces
{
    public interface IEntityRequestLog
    {
        IPAddress IPAddress { get; set; }
        string UserAgent { get; set; }
    }
}