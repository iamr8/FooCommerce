using System.Net;

namespace FooCommerce.Domain.Entities;

public interface IEntityRequestTrackable
{
    IPAddress IPAddress { get; init; }
    string UserAgent { get; init; }
}