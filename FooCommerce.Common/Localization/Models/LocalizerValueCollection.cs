using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

using FooCommerce.Common.JsonConverters.Helpers;

namespace FooCommerce.Common.Localization.Models;

[JsonConverter(typeof(JsonLocalizerValueCollectionToObjectConverter))]
public class LocalizerValueCollection : SortedList<CultureInfo, string>
{
    public LocalizerValueCollection(IDictionary<CultureInfo, string> dictionary) : base(dictionary)
    {
    }

    public LocalizerValueCollection()
    {
    }

    /// <summary>Gets or sets the value associated with the specified key.</summary>
    /// <param name="culture">The key whose value to get or set.</param>
    /// <exception cref="KeyNotFoundException">The property is retrieved and <paramref name="culture" /> does not exist in the collection.</exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="CultureNotFoundException"></exception>
    /// <returns>The value associated with the specified key. If the specified key is not found, a get operation throws a <see cref="T:System.Collections.Generic.KeyNotFoundException" /> and a set operation creates a new element using the specified key.</returns>
    public string this[string culture]
    {
        get
        {
            var cultureInfo = CultureInfo.GetCultureInfo(culture);
            return this[cultureInfo];
        }
    }

    public new string this[CultureInfo culture]
    {
        get
        {
            if (this.TryGetValue(CultureInfo.CurrentCulture, out var s))
            {
                return s;
            }

            return "";
        }
    }

    public override string ToString()
    {
        return this[CultureInfo.CurrentCulture];
    }

    public static explicit operator string(LocalizerValueCollection value)
    {
        return value[CultureInfo.CurrentCulture];
    }

    internal class JsonLocalizerValueCollectionToObjectConverter : JsonConverter<LocalizerValueCollection>
    {
        public override LocalizerValueCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw ThrowHelper.GenerateJsonException_DeserializeUnableToConvertValue(typeof(LocalizerValueCollection));

            var dict = new Dictionary<CultureInfo, string>();
            var cultureString = string.Empty;
            while (reader.Read())
            {
                if (reader.TokenType != JsonTokenType.StartObject &&
                    reader.TokenType != JsonTokenType.EndObject &&
                    reader.TokenType != JsonTokenType.String &&
                    reader.TokenType != JsonTokenType.PropertyName)
                    throw ThrowHelper.GenerateJsonException_DeserializeUnableToConvertValue(typeof(LocalizerValueCollection));

                if (reader.TokenType != JsonTokenType.String && reader.TokenType != JsonTokenType.PropertyName)
                    continue;

                if (string.IsNullOrEmpty(cultureString) && reader.TokenType == JsonTokenType.PropertyName)
                {
                    cultureString = reader.GetString();
                }
                else
                {
                    if (reader.TokenType != JsonTokenType.String)
                        throw ThrowHelper.GenerateJsonException_DeserializeUnableToConvertValue(typeof(LocalizerValueCollection));

                    var value = reader.GetString();

                    var culture = CultureInfo.GetCultureInfo(cultureString);
                    dict.Add(culture, value);

                    cultureString = null;
                }
            }

            var output = new LocalizerValueCollection(dict);
            return output;
        }

        public override void Write(Utf8JsonWriter writer, LocalizerValueCollection value, JsonSerializerOptions options)
        {
            var element = new JsonObject();
            for (var i = 0; i < value.Count; i++)
            {
                var (culture, s) = value.ElementAt(i);
                element.Add(culture.TwoLetterISOLanguageName, s);
            }
            writer.WriteStringValue(element.ToJsonString(options));
        }
    }
}