using System.Text.Json;
using System.Text.Json.Serialization;

namespace FooCommerce.Common.JsonConverters;

public class JsonDateTimeToUnixConverter : JsonConverter<DateTime>
{
    private static readonly DateTime _epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetInt64();
        return _epoch.AddMilliseconds(value / 1000d);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue((value - _epoch).TotalMilliseconds + "000");
    }
}