using System.Globalization;
using System.Net;

using NodaTime;

namespace FooCommerce.Application;

public interface IEndUser
{
    IPAddress IPAddress { get; set; }
    string UserAgent { get; set; }
    RegionInfo Country { get; set; }
    DateTimeZone Timezone { get; set; }
}