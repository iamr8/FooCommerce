using Autofac;

using FooCommerce.Application.Models;

using Microsoft.AspNetCore.Http;

using NodaTime;

namespace FooCommerce.Application.Helpers;

public static class DateTimeHelper
{
    /// <summary>
    /// Returns a localized datetime according to the given Timezone.
    /// </summary>
    /// <param name="utcDateTime"></param>
    /// <param name="endUser"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static DateTime ToLocal(this DateTime utcDateTime, EndUser endUser)
    {
        if (endUser == null)
            throw new ArgumentNullException(nameof(endUser));
        if (utcDateTime.Kind != DateTimeKind.Utc)
            throw new ArgumentException($"{nameof(utcDateTime)} must be in kind of UTC.");

        var instant = Instant.FromDateTimeUtc(utcDateTime);
        if (endUser.Timezone == null)
            throw new NullReferenceException("Unable to get user's timezone.");

        var result = instant.InZone(endUser.Timezone).ToDateTimeUnspecified();
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

        var endUser = httpContext.GetEndUser() ?? throw new ArgumentNullException("httpContext.GetEndUser()");
        return utcDateTime.ToLocal(endUser);
    }

    /// <summary>
    /// Returns a localized datetime according to the given Timezone.
    /// </summary>
    /// <param name="utcDateTime"></param>
    /// <param name="container"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static DateTime ToLocal(this DateTime utcDateTime, IContainer container)
    {
        if (container == null)
            throw new ArgumentNullException(nameof(container));

        if (utcDateTime.Kind != DateTimeKind.Utc)
            throw new ArgumentException($"{nameof(utcDateTime)} must be in kind of UTC.");

        var httpContextAccessor = container.Resolve<IHttpContextAccessor>() ?? throw new ArgumentNullException("container.Resolve<IHttpContextAccessor>()");
        var httpContext = httpContextAccessor.HttpContext ?? throw new ArgumentNullException("httpContextAccessor.HttpContext");
        return utcDateTime.ToLocal(httpContext);
    }
}