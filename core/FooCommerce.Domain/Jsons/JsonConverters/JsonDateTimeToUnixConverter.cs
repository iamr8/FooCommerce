using System.Text.Json;
using System.Text.Json.Serialization;

using FooCommerce.Domain.Helpers;

namespace FooCommerce.Domain.Jsons.JsonConverters;

public class JsonDateTimeToUnixConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTimeHelper.FromUnixTime(reader.GetInt64());
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(DateTimeHelper.ToUnixTime(value));
    }
}