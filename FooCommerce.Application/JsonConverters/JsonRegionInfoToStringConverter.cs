using System.Globalization;

using Newtonsoft.Json;

namespace FooCommerce.Application.JsonConverters;

public class JsonRegionInfoToStringConverter : JsonConverter<RegionInfo>
{
    public override async void WriteJson(JsonWriter writer, RegionInfo value, JsonSerializer serializer)
    {
        if (value == null)
            return;

        var culture = value.TwoLetterISORegionName;
        await writer.WriteValueAsync(culture);
    }

    public override RegionInfo ReadJson(JsonReader reader, Type objectType, RegionInfo existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var twoIso = reader.Value?.ToString();
        if (string.IsNullOrEmpty(twoIso))
            return null;

        return new RegionInfo(twoIso);
    }
}