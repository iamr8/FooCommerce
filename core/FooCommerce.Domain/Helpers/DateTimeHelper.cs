namespace FooCommerce.Domain.Helpers;

public static class DateTimeHelper
{
    public static long ToUnixTime(this DateTime datetime)
    {
        var timestamp = (long)datetime.Subtract(DateTime.UnixEpoch).TotalSeconds;
        return timestamp;
    }

    public static DateTime FromUnixTime(long seconds)
    {
        var timestamp = DateTime.UnixEpoch.AddSeconds(seconds);
        return timestamp;
    }
}