using FooCommerce.Common.HttpContextRequest;

using Microsoft.AspNetCore.Http;

using NodaTime;

namespace FooCommerce.Common.Helpers;

public static class DateTimeHelper
{
    /// <summary>
    /// Returns a localized datetime according to the given Timezone.
    /// </summary>
    /// <param name="utcDateTime"></param>
    /// <param name="httpRequestInfo"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static DateTime ToLocal(this DateTime utcDateTime, IHttpRequestInfo httpRequestInfo)
    {
        if (httpRequestInfo == null)
            throw new ArgumentNullException(nameof(httpRequestInfo));
        if (utcDateTime.Kind != DateTimeKind.Utc)
            throw new ArgumentException($"{nameof(utcDateTime)} must be in kind of UTC.");

        var instant = Instant.FromDateTimeUtc(utcDateTime);
        if (httpRequestInfo.Timezone == null)
            throw new NullReferenceException("Unable to get user's timezone.");

        var result = instant.InZone(httpRequestInfo.Timezone).ToDateTimeUnspecified();
        return result;
    }

    /// <summary>
    /// Returns a localized datetime according to the given Timezone.
    /// </summary>
    /// <param name="utcDateTime"></param>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static DateTime ToLocal(this DateTime utcDateTime, HttpContext httpContext)
    {
        if (httpContext == null)
            throw new ArgumentNullException(nameof(httpContext));
        if (utcDateTime.Kind != DateTimeKind.Utc)
            throw new ArgumentException($"{nameof(utcDateTime)} must be in kind of UTC.");

        var endUser = httpContext.GetRequestInfo() ?? throw new ArgumentNullException("httpContext.GetEndUser()");
        return utcDateTime.ToLocal(endUser);
    }
}