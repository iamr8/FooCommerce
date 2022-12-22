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
    /// <param name="timezoneInfo"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static DateTime ToLocal(this DateTime utcDateTime, ITimezone timezoneInfo)
    {
        if (timezoneInfo == null)
            throw new ArgumentNullException(nameof(timezoneInfo));
        if (utcDateTime.Kind != DateTimeKind.Utc)
            throw new ArgumentException($"{nameof(utcDateTime)} must be in kind of UTC.");

        var timezone = timezoneInfo.GetTimezone();
        var result = utcDateTime.ToLocal(timezone);
        return result;
    }

    public static DateTimeZone? GetTimezone(this ITimezone timezoneInfo)
    {
        if (!string.IsNullOrEmpty(timezoneInfo.TimezoneId))
            return DateTimeZoneProviders.Tzdb[timezoneInfo.TimezoneId];

        if (timezoneInfo.Country == null)
            return null;

        if (TzdbDateTimeZoneSource.Default.ZoneLocations == null)
            return null;

        var zoneIds = TzdbDateTimeZoneSource.Default.ZoneLocations
            .Where(x => x.CountryCode == timezoneInfo.Country.TwoLetterISORegionName)
            .Select(x => x.ZoneId)
            .ToList();
        if (zoneIds.Any())
            return DateTimeZoneProviders.Tzdb[zoneIds[0]];

        return null;
    }
}