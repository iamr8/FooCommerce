#nullable enable

using System.Globalization;
using System.Net;

using FooCommerce.Application.Interfaces;
using FooCommerce.Application.JsonConverters;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using NodaTime;
using NodaTime.TimeZones;

namespace FooCommerce.Application.Models;

public record EndUser : IEndUser
{
    public EndUser()
    {
    }

    public EndUser(HttpContext httpContext)
    {
        this.IPAddress = this.GetIPAddress(httpContext);
        this.UserAgent = GetUserAgent(httpContext);
        this.Country = GetCountry(httpContext);
        this.Timezone = this.GetTimezone(httpContext);
    }

    private DateTimeZone? GetTimezone(HttpContext httpContext)
    {
        if (httpContext.Request.Cookies.TryGetValue("timeZone", out var timeZoneStr))
            return DateTimeZoneProviders.Tzdb[timeZoneStr];

        if (this.Country == null)
            return null;

        if (TzdbDateTimeZoneSource.Default.ZoneLocations == null)
            return null;

        var zoneIds = TzdbDateTimeZoneSource.Default.ZoneLocations
            .Where(x => x.CountryCode == this.Country.TwoLetterISORegionName)
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
        if (!httpContext.Request.Headers.TryGetValue("UserAgent", out var userAgent))
            return null;

        return userAgent;
    }
    private IPAddress? GetIPAddress(HttpContext httpContext)
    {
        if (httpContext.Request.Headers.TryGetValue("CF-Connecting-IP", out var cloudflareConnectingIP) && !string.IsNullOrEmpty(cloudflareConnectingIP))
            IPAddress_Cloudflare = IPAddress.Parse(cloudflareConnectingIP.ToString());

        if (httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var xForwardedFor) && !string.IsNullOrEmpty(xForwardedFor))
            IPAddress_XForwardedFor = IPAddress.Parse(xForwardedFor.ToString());

        if (httpContext.Connection.RemoteIpAddress != null && !IPAddress.IsLoopback(httpContext.Connection.RemoteIpAddress))
            IPAddress_RemoteConnection = httpContext.Connection.RemoteIpAddress;

        if (httpContext.Connection.LocalIpAddress != null && !IPAddress.IsLoopback(httpContext.Connection.LocalIpAddress))
            IPAddress_LocalConnection = httpContext.Connection.RemoteIpAddress;

        if (IPAddress_Cloudflare != null)
            return IPAddress_Cloudflare;

        if (IPAddress_XForwardedFor != null)
            return IPAddress_XForwardedFor;

        if (IPAddress_RemoteConnection != null)
            return IPAddress_RemoteConnection;

        return IPAddress_LocalConnection;
    }

    private IPAddress? IPAddress_Cloudflare;
    private IPAddress? IPAddress_XForwardedFor;
    private IPAddress? IPAddress_RemoteConnection;
    private IPAddress? IPAddress_LocalConnection;

    [JsonConverter(typeof(JsonIPAddressToStringConverter))]
    public IPAddress? IPAddress { get; set; }

    public string? UserAgent { get; set; }

    [JsonConverter(typeof(JsonRegionInfoToStringConverter))]
    public RegionInfo? Country { get; set; }

    [JsonConverter(typeof(JsonDateTimeZoneToStringConverter))]
    public DateTimeZone? Timezone { get; set; }
}