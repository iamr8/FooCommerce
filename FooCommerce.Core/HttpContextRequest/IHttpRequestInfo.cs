using System.Globalization;
using System.Net;

using NodaTime;

namespace FooCommerce.Core.HttpContextRequest;

public interface IHttpRequestInfo
{
    IPAddress IPAddress { get; set; }
    string UserAgent { get; set; }
    RegionInfo Country { get; set; }
    DateTimeZone Timezone { get; set; }
    HttpRequestBrowser Browser { get; set; }
    HttpRequestEngine Engine { get; set; }
    HttpRequestDevice Device { get; set; }
    HttpRequestPlatform Platform { get; set; }
}