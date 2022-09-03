using System.Text.Json;
using System.Text.Json.Serialization;

namespace FooCommerce.Common.JsonConverters;

public class JsonDateTimeToUnixConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var timestamp = DateTime.UnixEpoch.AddSeconds(reader.GetInt64());
        return timestamp;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue((long)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds);
    }
}