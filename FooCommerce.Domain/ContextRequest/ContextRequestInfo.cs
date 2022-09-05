#nullable enable

using System.Globalization;
using System.Net;

namespace FooCommerce.Domain.ContextRequest;

public record ContextRequestInfo : IContextRequestInfo
{
    public IPAddress? IPAddress { get; set; }

    public string? UserAgent { get; set; }

    public RegionInfo? Country { get; set; }

    public string TimezoneId { get; set; }
    public IDictionary<string, object> Items { get; } = new Dictionary<string, object>();
    public ContextRequestBrowser Browser { get; set; }
    public ContextRequestEngine Engine { get; set; }

    public ContextRequestPlatform Platform { get; set; }

    public ContextRequestDevice Device { get; set; }
}