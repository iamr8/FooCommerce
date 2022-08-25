using Newtonsoft.Json;

using NodaTime;

namespace FooCommerce.Application.JsonConverters;

public class JsonDateTimeZoneToStringConverter : JsonConverter<DateTimeZone>
{
    public override void WriteJson(JsonWriter writer, DateTimeZone value, JsonSerializer serializer)
    {
        writer.WriteValue(value.Id);
    }

    public override DateTimeZone ReadJson(JsonReader reader, Type objectType, DateTimeZone existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var id = reader.Value?.ToString();
        if (string.IsNullOrEmpty(id))
            return null;

        return DateTimeZoneProviders.Tzdb[id];
    }
}