using Newtonsoft.Json;

namespace FooCommerce.Infrastructure.JsonCustomization.Converters;

public class JsonDateTimeToUnixConverter : JsonConverter<DateTime>
{
    public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var ticks = (long)reader.Value;
        return DateTimeOffset.FromUnixTimeSeconds(ticks).UtcDateTime;
    }

    public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
    {
        var ticks = new DateTimeOffset(value).ToUnixTimeSeconds();
        writer.WriteValue(ticks);
    }
}