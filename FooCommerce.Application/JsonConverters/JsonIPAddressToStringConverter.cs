using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FooCommerce.Application.JsonConverters;

public class JsonIPAddressToStringConverter : JsonConverter<IPAddress>
{
    public override IPAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var ip = reader.GetString();

        return string.IsNullOrEmpty(ip)
            ? IPAddress.None
            : IPAddress.Parse(ip);
    }

    public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options)
    {
        var plainIp = value == null || IPAddress.IsLoopback(value)
            ? null
            : value.ToString();
        writer.WriteStringValue(plainIp);
    }
}