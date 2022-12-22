using System.Globalization;

namespace FooCommerce.Domain.ContextRequest;

public interface ITimezone
{
    RegionInfo Country { get; }

    string TimezoneId { get; }
}