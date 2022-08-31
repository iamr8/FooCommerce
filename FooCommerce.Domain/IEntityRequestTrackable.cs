using System.Net;

namespace FooCommerce.Domain;

public interface IEntityRequestTrackable
{
    IPAddress IPAddress { get; init; }
    string UserAgent { get; init; }
}