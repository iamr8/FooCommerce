using FooCommerce.AspNetCoreExtensions.Helpers;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Localization.AspNetCoreExtensions;

public static class DateTimeHelper
{
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
        return Helpers.DateTimeHelper.ToLocal(utcDateTime, endUser);
    }

    /// <summary>
    /// Returns a localized datetime according to the given Timezone.
    /// </summary>
    /// <param name="utcDateTime"></param>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static DateTime ToLocal(this DateTime utcDateTime, IServiceProvider serviceProvider)
    {
        if (serviceProvider == null)
            throw new ArgumentNullException(nameof(serviceProvider));

        if (utcDateTime.Kind != DateTimeKind.Utc)
            throw new ArgumentException($"{nameof(utcDateTime)} must be in kind of UTC.");

        var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
        var httpContext = httpContextAccessor.HttpContext ?? throw new ArgumentNullException("httpContextAccessor.HttpContext");
        return utcDateTime.ToLocal(httpContext);
    }
}