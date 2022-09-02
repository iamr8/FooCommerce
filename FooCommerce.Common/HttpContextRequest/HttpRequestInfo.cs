#nullable enable

using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

using FooCommerce.Common.JsonConverters;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;

using NodaTime;
using NodaTime.TimeZones;

using Wangkanai.Detection;
using Wangkanai.Detection.Services;

namespace FooCommerce.Common.HttpContextRequest;

public record HttpRequestInfo : IHttpRequestInfo
{
    private HttpContext _httpContext;

    public HttpRequestInfo()
    {
    }

    public HttpRequestInfo(HttpContext httpContext)
    {
        _httpContext = httpContext;
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

    public void SetHttpContext(HttpContext httpContext)
    {
        this._httpContext = httpContext;
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

    /// <summary>
    /// Adds website endpoint to <see cref="HttpContext"/> for future uses.
    /// </summary>
    /// <remarks>This code must be appended right after <code>app.UseRouting()</code> in <c>Program.cs</c></remarks>
    /// <param name="app"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void UseWebsiteUrl(ref WebApplication app)
    {
        if (app == null) throw new ArgumentNullException(nameof(app));
        app.Use(async (context, next) =>
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                context.Items.Add("WebsiteURL", endpoint.Metadata);
            }

            await next();
        });
    }
    /// <summary>
    /// Returns endpoint url of the web app.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public string GetWebsiteUrl()
    {
        if (_httpContext == null)
            throw new NullReferenceException(
                $"field {nameof(_httpContext)} is null. It can be set simply by using {nameof(SetHttpContext)} method.");

        var scheme = _httpContext.Request.Scheme;
        var webEnvironment = _httpContext.RequestServices.GetService<IWebHostEnvironment>();
        if (webEnvironment.IsDevelopment())
            return $"{scheme}://{_httpContext.Request.Host}{_httpContext.Request.PathBase}";

        if (_httpContext.Request.Headers.TryGetValue("CF-Visitor", out var cloudFlareVisitor))
        {
            scheme = JsonSerializer.Deserialize<dynamic>(cloudFlareVisitor)!.scheme;
        }
        else
        {
            if (_httpContext.Request.Headers.TryGetValue("X-Forwarded-Proto", out var xForwarderProto))
            {
                scheme = xForwarderProto;
            }
        }

        var hasWebsiteUrl = _httpContext.Items.TryGetValue("WebsiteURL", out var websiteUrl);
        if (!hasWebsiteUrl)
            throw new NullReferenceException("Unable to get Website url from HttpContext.");

        return $"{scheme}://{(webEnvironment.IsProduction() ? "www." : "")}{websiteUrl}/";
    }
}