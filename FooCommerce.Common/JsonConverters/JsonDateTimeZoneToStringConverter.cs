using System.Text.Json;
using System.Text.Json.Serialization;

using NodaTime;

namespace FooCommerce.Common.JsonConverters;

public class JsonDateTimeZoneToStringConverter : JsonConverter<DateTimeZone>
{
    public override DateTimeZone Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var id = reader.GetString();
        if (string.IsNullOrEmpty(id))
            return null;

        return DateTimeZoneProviders.Tzdb[id];
    }

    public override void Write(Utf8JsonWriter writer, DateTimeZone value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Id);
    }
}