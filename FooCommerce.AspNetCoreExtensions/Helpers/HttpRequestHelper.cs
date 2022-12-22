using System.Globalization;
using System.Net;

using FooCommerce.Domain.ContextRequest;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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

    public static ContextRequestInfo GetRequestInfo(this HttpContext context) => GetInstance(context);

    public static ContextRequestInfo GetInstance(HttpContext httpContext)
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

    private static RegionInfo GetCountry(this HttpContext httpContext)
    {
        if (!httpContext.Request.Headers.TryGetValue("CF-IPCountry", out var countryCode))
            return RegionInfo.CurrentRegion;

        if (string.IsNullOrEmpty(countryCode) || countryCode == "T1" || countryCode == "XX")
            return null;

        return new RegionInfo(countryCode!);
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
            _ipAddressLocalConnection = httpContext.Connection.LocalIpAddress;

        if (_ipAddressCloudflare != null)
            return _ipAddressCloudflare;

        if (_ipAddressXForwardedFor != null)
            return _ipAddressXForwardedFor;

        if (_ipAddressRemoteConnection != null)
            return _ipAddressRemoteConnection;

        return _ipAddressLocalConnection;
    }
}