using System.Globalization;
using System.Net;

namespace FooCommerce.Domain.ContextRequest;

public interface IContextRequestInfo
{
    IPAddress IPAddress { get; }
    string UserAgent { get; }
    RegionInfo? Country { get; }
    ContextRequestBrowser Browser { get; }
    ContextRequestEngine Engine { get; }

    ContextRequestPlatform Platform { get; }

    ContextRequestDevice Device { get; }
    string TimezoneId { get; set; }
    IDictionary<string, object> Items { get; }
}