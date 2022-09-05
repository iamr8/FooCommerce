using System.Globalization;
using System.Net;
using System.Text.Json;

using FooCommerce.Domain.ContextRequest;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;

using Wangkanai.Detection;
using Wangkanai.Detection.Services;

namespace FooCommerce.AspNetCoreExtensions.Helpers;

public static class ContextRequestHelper
{
    // ReSharper disable once IdentifierTypo
    private static IPAddress? _ipAddressCloudflare;

    private static IPAddress? _ipAddressXForwardedFor;
    private static IPAddress? _ipAddressRemoteConnection;
    private static IPAddress? _ipAddressLocalConnection;

    public static IContextRequestInfo GetRequestInfo(this HttpContext context) => GetInstance(context);

    public static IContextRequestInfo GetInstance(HttpContext httpContext)
    {
        var instance = new ContextRequestInfo
        {
            TimezoneId = httpContext.GetTimezone(),
            IPAddress = httpContext.GetIPAddress(),
            UserAgent = httpContext.GetUserAgent(),
            Country = httpContext.GetCountry(),
        };
        instance.GetDetection(httpContext);
        return instance;
    }

    private static string? GetTimezone(this HttpContext httpContext)
    {
        if (httpContext.Request.Cookies.TryGetValue("timeZone", out var timeZoneStr))
            return timeZoneStr;

        return null;
    }

    public static void GetDetection(this ContextRequestInfo requestInfo, HttpContext httpContext)
    {
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

        requestInfo.Browser = new ContextRequestBrowser(detectionService.Browser.Name.ToString(), detectionService.Browser.Version.ToString());
        requestInfo.Device = new ContextRequestDevice(detectionService.Device.Type.ToString());
        requestInfo.Platform = new ContextRequestPlatform(detectionService.Platform.Name.ToString(), detectionService.Platform.Version.ToString());
        requestInfo.Engine = new ContextRequestEngine(detectionService.Engine.Name.ToString());
    }

    // public static void SetHttpContext(HttpContext httpContext)
    // {
    // this._httpContext = httpContext;
    // }

    /// <summary>
    /// Returns endpoint url of the web app.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    public static string GetWebsiteUrl(this HttpContext httpContext)
    {
        if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

        var scheme = httpContext.Request.Scheme;
        var webEnvironment = httpContext.RequestServices.GetService<IWebHostEnvironment>();
        if (webEnvironment.IsDevelopment())
            return $"{scheme}://{httpContext.Request.Host}{httpContext.Request.PathBase}";

        if (httpContext.Request.Headers.TryGetValue("CF-Visitor", out var cloudFlareVisitor))
        {
            scheme = JsonSerializer.Deserialize<dynamic>(cloudFlareVisitor)!.scheme;
        }
        else
        {
            if (httpContext.Request.Headers.TryGetValue("X-Forwarded-Proto", out var xForwarderProto))
            {
                scheme = xForwarderProto;
            }
        }

        var hasWebsiteUrl = httpContext.Items.TryGetValue("WebsiteURL", out var websiteUrl);
        if (!hasWebsiteUrl)
            throw new NullReferenceException("Unable to get Website url from HttpContext.");

        return $"{scheme}://{(webEnvironment.IsProduction() ? "www." : "")}{websiteUrl}/";
    }

    private static RegionInfo GetCountry(this HttpContext httpContext)
    {
        if (!httpContext.Request.Headers.TryGetValue("CF-IPCountry", out var countryCode))
            return RegionInfo.CurrentRegion;

        if (string.IsNullOrEmpty(countryCode) || countryCode == "T1" || countryCode == "XX")
            return null;

        return new RegionInfo(countryCode!);
    }

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

    private static string? GetUserAgent(this HttpContext httpContext)
    {
        if (!httpContext.Request.Headers.TryGetValue(HeaderNames.UserAgent, out var userAgent))
            return null;

        return userAgent;
    }

    private static IPAddress? GetIPAddress(this HttpContext httpContext)
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
}