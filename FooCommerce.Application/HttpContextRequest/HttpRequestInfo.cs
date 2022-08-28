#nullable enable

using System.Globalization;
using System.Net;
using System.Text.Json.Serialization;

using FooCommerce.Application.JsonConverters;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

using NodaTime;
using NodaTime.TimeZones;

using Wangkanai.Detection;
using Wangkanai.Detection.Services;

namespace FooCommerce.Application.HttpContextRequest;

public record HttpRequestInfo : IHttpRequestInfo
{
    public HttpRequestInfo()
    {
    }

    public HttpRequestInfo(HttpContext httpContext)
    {
        IPAddress = GetIPAddress(httpContext);
        UserAgent = GetUserAgent(httpContext);
        Country = GetCountry(httpContext);
        Timezone = GetTimezone(httpContext);

        var detectionService = httpContext.RequestServices.GetService<IDetectionService>();
        if (detectionService == null)
        {
            var userAgentService = new UserAgentService(new HttpContextService(new HttpContextAccessor { HttpContext = httpContext }));
            var detectionOptions = new DetectionOptions();
            var deviceService = new DeviceService(userAgentService);
            var crawlerService = new CrawlerService(userAgentService, detectionOptions);
            var platformService = new PlatformService(userAgentService);
            var engineService = new EngineService(userAgentService, platformService);
            var browserService = new BrowserService(userAgentService, engineService);

            detectionService = new DetectionService(
                userAgentService,
                deviceService,
                crawlerService,
                browserService,
                engineService,
                platformService);
        }

        Browser = new HttpRequestBrowser(detectionService.Browser.Name, detectionService.Browser.Version);
        Device = new HttpRequestDevice(detectionService.Device.Type);
        Platform = new HttpRequestPlatform(detectionService.Platform.Name, detectionService.Platform.Version);
        Engine = new HttpRequestEngine(detectionService.Engine.Name);
    }

    private DateTimeZone? GetTimezone(HttpContext httpContext)
    {
        if (httpContext.Request.Cookies.TryGetValue("timeZone", out var timeZoneStr))
            return DateTimeZoneProviders.Tzdb[timeZoneStr];

        if (Country == null)
            return null;

        if (TzdbDateTimeZoneSource.Default.ZoneLocations == null)
            return null;

        var zoneIds = TzdbDateTimeZoneSource.Default.ZoneLocations
            .Where(x => x.CountryCode == Country.TwoLetterISORegionName)
            .Select(x => x.ZoneId)
            .ToList();
        if (zoneIds.Any())
            return DateTimeZoneProviders.Tzdb[zoneIds.First()];

        return null;
    }
    private static RegionInfo GetCountry(HttpContext httpContext)
    {
        if (!httpContext.Request.Headers.TryGetValue("CF-IPCountry", out var countryCode))
            return RegionInfo.CurrentRegion;

        if (string.IsNullOrEmpty(countryCode) || countryCode == "T1" || countryCode == "XX")
            return null;

        return new RegionInfo(countryCode!);
    }
    private static string? GetUserAgent(HttpContext httpContext)
    {
        if (!httpContext.Request.Headers.TryGetValue(HeaderNames.UserAgent, out var userAgent))
            return null;

        return userAgent;
    }
    private IPAddress? GetIPAddress(HttpContext httpContext)
    {
        if (httpContext.Request.Headers.TryGetValue("CF-Connecting-IP", out var cloudflareConnectingIP) && !string.IsNullOrEmpty(cloudflareConnectingIP))
            _ipAddressCloudflare = IPAddress.Parse(cloudflareConnectingIP.ToString());

        if (httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var xForwardedFor) && !string.IsNullOrEmpty(xForwardedFor))
            _ipAddressXForwardedFor = IPAddress.Parse(xForwardedFor.ToString());

        if (httpContext.Connection.RemoteIpAddress != null && !IPAddress.IsLoopback(httpContext.Connection.RemoteIpAddress))
            _ipAddressRemoteConnection = httpContext.Connection.RemoteIpAddress;

        if (httpContext.Connection.LocalIpAddress != null && !IPAddress.IsLoopback(httpContext.Connection.LocalIpAddress))
            _ipAddressLocalConnection = httpContext.Connection.RemoteIpAddress;

        if (_ipAddressCloudflare != null)
            return _ipAddressCloudflare;

        if (_ipAddressXForwardedFor != null)
            return _ipAddressXForwardedFor;

        if (_ipAddressRemoteConnection != null)
            return _ipAddressRemoteConnection;

        return _ipAddressLocalConnection;
    }

    // ReSharper disable once IdentifierTypo
    private IPAddress? _ipAddressCloudflare;
    private IPAddress? _ipAddressXForwardedFor;
    private IPAddress? _ipAddressRemoteConnection;
    private IPAddress? _ipAddressLocalConnection;

    [JsonConverter(typeof(JsonIPAddressToStringConverter))]
    public IPAddress? IPAddress { get; set; }

    public string? UserAgent { get; set; }

    [JsonConverter(typeof(JsonRegionInfoToStringConverter))]
    public RegionInfo? Country { get; set; }

    [JsonConverter(typeof(JsonDateTimeZoneToStringConverter))]
    public DateTimeZone? Timezone { get; set; }

    public HttpRequestBrowser Browser { get; set; }
    public HttpRequestEngine Engine { get; set; }

    public HttpRequestPlatform Platform { get; set; }

    public HttpRequestDevice Device { get; set; }
}