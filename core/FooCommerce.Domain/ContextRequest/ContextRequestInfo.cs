#nullable enable

using System.Globalization;
using System.Net;
using System.Text.Json.Serialization;

using FooCommerce.Domain.Jsons.JsonConverters;

namespace FooCommerce.Domain.ContextRequest;

[Serializable]
[JsonSerializable(typeof(ContextRequestBrowser))]
[JsonSerializable(typeof(ContextRequestEngine))]
[JsonSerializable(typeof(ContextRequestPlatform))]
[JsonSerializable(typeof(ContextRequestDevice))]
public sealed record ContextRequestInfo : ITimezone
{
    [JsonConverter(typeof(JsonIPAddressToStringConverter))]
    public IPAddress? IPAddress { get; set; }

    public string? UserAgent { get; set; }

    [JsonConverter(typeof(JsonRegionInfoToStringConverter))]
    public RegionInfo? Country { get; set; }
    [JsonConverter(typeof(JsonCultureToStringConverter))]
    public CultureInfo Culture { get; set; }

    public string TimezoneId { get; set; }
    public ContextRequestBrowser Browser { get; set; }
    public ContextRequestEngine Engine { get; set; }

    public ContextRequestPlatform Platform { get; set; }

    public ContextRequestDevice Device { get; set; }
}