using System.Text.Json;
using System.Text.Json.Serialization;
using FooCommerce.Common.Helpers;
using Microsoft.AspNetCore.Html;

namespace FooCommerce.Common.JsonConverters;

public class JsonHtmlContentToStringConverter : JsonConverter<IHtmlContent>
{
    public override IHtmlContent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value == null || string.IsNullOrEmpty(value))
            return null;

        var html = new HtmlString(value);
        return html;
    }

    public override void Write(Utf8JsonWriter writer, IHtmlContent value, JsonSerializerOptions options)
    {
        if (value == null)
            writer.WriteNullValue();

        var html = value.GetString();
        writer.WriteStringValue(html);
    }
}