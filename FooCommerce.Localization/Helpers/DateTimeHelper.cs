using FooCommerce.Domain.ContextRequest;

using NodaTime;
using NodaTime.TimeZones;

namespace FooCommerce.Localization.Helpers;

public static class DateTimeHelper
{
    /// <summary>
    /// Returns a localized datetime according to the given Timezone.
    /// </summary>
    /// <param name="utcDateTime"></param>
    /// <param name="timezone"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static DateTime ToLocal(this DateTime utcDateTime, DateTimeZone timezone)
    {
        if (utcDateTime.Kind != DateTimeKind.Utc)
            throw new ArgumentException($"{nameof(utcDateTime)} must be in kind of UTC.");

        var instant = Instant.FromDateTimeUtc(utcDateTime);
        var result = instant.InZone(timezone).ToDateTimeUnspecified();
        return result;
    }

    /// <summary>
    /// Returns a localized datetime according to the given Timezone.
    /// </summary>
    /// <param name="utcDateTime"></param>
    /// <param name="contextRequestInfo"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static DateTime ToLocal(this DateTime utcDateTime, IContextRequestInfo contextRequestInfo)
    {
        if (contextRequestInfo == null)
            throw new ArgumentNullException(nameof(contextRequestInfo));
        if (utcDateTime.Kind != DateTimeKind.Utc)
            throw new ArgumentException($"{nameof(utcDateTime)} must be in kind of UTC.");

        var timezone = contextRequestInfo.GetTimezone();
        var result = utcDateTime.ToLocal(timezone);
        return result;
    }

    public static DateTimeZone? GetTimezone(this IContextRequestInfo requestInfo)
    {
        if (!string.IsNullOrEmpty(requestInfo.TimezoneId))
            return DateTimeZoneProviders.Tzdb[requestInfo.TimezoneId];

        if (requestInfo.Country == null)
            return null;

        if (TzdbDateTimeZoneSource.Default.ZoneLocations == null)
            return null;

        var zoneIds = TzdbDateTimeZoneSource.Default.ZoneLocations
            .Where(x => x.CountryCode == requestInfo.Country.TwoLetterISORegionName)
            .Select(x => x.ZoneId)
            .ToList();
        if (zoneIds.Any())
            return DateTimeZoneProviders.Tzdb[zoneIds.First()];

        return null;
    }
}