using System.Net;

namespace FooCommerce.Domain.Interfaces.Database
{
    public interface IEntityRequestLog
    {
        IPAddress IPAddress { get; set; }
        string UserAgent { get; set; }
    }
}