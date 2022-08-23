using System.Net;

namespace FooCommerce.Domain.Interfaces.Database;

public interface IEntityRequestTrackable
{
    IPAddress IPAddress { get; init; }
    string UserAgent { get; init; }
}