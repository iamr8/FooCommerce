using System.Net;

namespace FooCommerce.Application.DbProvider.Interfaces
{
    public interface IEntityRequestLog
    {
        IPAddress IPAddress { get; set; }
        string UserAgent { get; set; }
    }
}