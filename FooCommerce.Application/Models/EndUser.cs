using System.Globalization;
using System.Net;

using Microsoft.AspNetCore.Http;

using NodaTime;
using NodaTime.TimeZones;

namespace FooCommerce.Application.Models;

public record EndUser(HttpContext HttpContext)
{
    private IPAddress? IP_CF_Connecting_IP { get; set; }
    private IPAddress? IP_X_Forwarded_For { get; set; }
    private IPAddress? IP_HttpContext_Connection_Remote { get; set; }
    private IPAddress? IP_HttpContext_Connection_Local { get; set; }

    public IPAddress? IPAddress
    {
        get
        {
            if (HttpContext.Request.Headers.TryGetValue("CF-Connecting-IP", out var cloudflareConnectingIP) && !string.IsNullOrEmpty(cloudflareConnectingIP))
                IP_CF_Connecting_IP = IPAddress.Parse(cloudflareConnectingIP);

            if (HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var xForwardedFor) && !string.IsNullOrEmpty(xForwardedFor))
                IP_X_Forwarded_For = IPAddress.Parse(xForwardedFor);

            if (HttpContext.Connection.RemoteIpAddress != null && !IPAddress.IsLoopback(HttpContext.Connection.RemoteIpAddress))
                IP_HttpContext_Connection_Remote = HttpContext.Connection.RemoteIpAddress;

            if (HttpContext.Connection.LocalIpAddress != null && !IPAddress.IsLoopback(HttpContext.Connection.LocalIpAddress))
                IP_HttpContext_Connection_Local = HttpContext.Connection.RemoteIpAddress;

            if (IP_CF_Connecting_IP != null)
                return IP_CF_Connecting_IP;

            if (IP_X_Forwarded_For != null)
                return IP_X_Forwarded_For;

            if (IP_HttpContext_Connection_Remote != null)
                return IP_HttpContext_Connection_Remote;

            if (IP_HttpContext_Connection_Local != null)
                return IP_HttpContext_Connection_Local;

            return null;
        }
    }

    public string? UserAgent
    {
        get
        {
            if (!HttpContext.Request.Headers.TryGetValue("UserAgent", out var userAgent))
                return null;

            return userAgent;
        }
    }
    public RegionInfo? Country
    {
        get
        {
            if (!HttpContext.Request.Headers.TryGetValue("CF-IPCountry", out var countryCode))
                return RegionInfo.CurrentRegion;

            if (string.IsNullOrEmpty(countryCode) || countryCode == "T1" || countryCode == "XX")
                return null;

            return new RegionInfo(countryCode);
        }
    }

    public DateTimeZone? Timezone
    {
        get
        {
            if (HttpContext.Request.Cookies.TryGetValue("timeZone", out var timeZoneStr))
                return DateTimeZoneProviders.Tzdb[timeZoneStr];

            if (this.Country != null)
            {
                var zoneIds = TzdbDateTimeZoneSource.Default.ZoneLocations
                    .Where(x => x.CountryCode == this.Country.TwoLetterISORegionName)
                    .Select(x => x.ZoneId)
                    .ToList();
                if (zoneIds.Any())
                    return DateTimeZoneProviders.Tzdb[zoneIds.First()];
            }

            return null;
        }
    }
}