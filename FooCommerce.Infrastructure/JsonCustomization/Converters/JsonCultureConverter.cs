using System.Globalization;

using Newtonsoft.Json;

namespace FooCommerce.Infrastructure.JsonCustomization.Converters;

public class JsonCultureConverter : JsonConverter<CultureInfo>
{
    public override async void WriteJson(JsonWriter writer, CultureInfo value, JsonSerializer serializer)
    {
        if (value == null)
            return;

        var culture = value.Name;
        await writer.WriteValueAsync(culture);
    }

    public override CultureInfo ReadJson(JsonReader reader, Type objectType, CultureInfo existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        return existingValue;
    }
}