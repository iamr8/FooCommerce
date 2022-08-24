using FooCommerce.Infrastructure.Helpers;

using Microsoft.AspNetCore.Html;

using Newtonsoft.Json;

namespace FooCommerce.Infrastructure.JsonCustomization.Converters;

public class JsonHtmlContentConverter : JsonConverter<IHtmlContent>
{
    public override void WriteJson(JsonWriter writer, IHtmlContent value, JsonSerializer serializer)
    {
        if (value == null)
            writer.WriteValue("");

        var html = value.GetString();
        writer.WriteValue(html);
    }

    public override IHtmlContent ReadJson(JsonReader reader, Type objectType, IHtmlContent existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var value = reader.Value?.ToString();
        if (value == null || string.IsNullOrEmpty(value))
            return null;

        var html = new HtmlString(value);
        return html;
    }
}