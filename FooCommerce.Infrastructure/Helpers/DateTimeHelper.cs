using Autofac;

using Microsoft.AspNetCore.Http;

namespace FooCommerce.Infrastructure.Helpers;

public static class DateTimeHelper
{
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
        return Common.Helpers.DateTimeHelper.ToLocal(utcDateTime, httpContext);
    }
}