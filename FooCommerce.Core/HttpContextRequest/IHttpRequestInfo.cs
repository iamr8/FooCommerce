using System.Globalization;
using System.Net;

using NodaTime;

namespace FooCommerce.Core.HttpContextRequest;

public interface IHttpRequestInfo
{
    IPAddress IPAddress { get; }
    string UserAgent { get; }
    RegionInfo Country { get; }
    DateTimeZone Timezone { get; }
    HttpRequestBrowser Browser { get; }
    HttpRequestEngine Engine { get; }

    HttpRequestPlatform Platform { get; }

    HttpRequestDevice Device { get; }
}