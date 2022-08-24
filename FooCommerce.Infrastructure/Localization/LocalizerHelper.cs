using System.Globalization;

using FooCommerce.Application.Models.Localization;
using FooCommerce.Domain.Interfaces;
using FooCommerce.Infrastructure.Helpers;

using Microsoft.AspNetCore.Html;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FooCommerce.Infrastructure.Localization;

public static class LocalizerHelper
{
    /// <summary>
    /// Returns a <see cref="IHtmlContent"/> object that representing replaced input tags with given tags.
    /// </summary>
    /// <param name="localizer">An instance of <see cref="ILocalizer"/>.</param>
    /// <param name="key">Name of specific key in <see cref="Dictionary{TKey,TValue}"/> that containing a localized text.</param>
    /// <param name="tags">A collection of html tags that should be replaced in given html string.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>A <see cref="IHtmlContent"/> object that representing a formatted html with given tags.</returns>
    public static IHtmlContent Html(this ILocalizer localizer, string key, params Func<string, IHtmlContent>[] tags)
    {
        if (localizer == null)
            throw new ArgumentNullException(nameof(localizer));
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));
        if (tags?.Any() != true)
            throw new ArgumentNullException(nameof(tags));

        var localized = localizer[key];
        return localized.ReplaceHtml(tags);
    }

    /// <summary>
    /// Returns a <see cref="IHtmlContent"/> object that representing formatted input.
    /// </summary>
    /// <param name="localizer">An instance of <see cref="ILocalizer"/>.</param>
    /// <param name="key">Name of specific key in <see cref="ILocalizer.Dictionary"/> that containing a localized text.</param>
    /// <param name="args">A collection of arguments that should be replaced in given text.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>A <see cref="IHtmlContent"/> object that representing a formatted text with given tags.</returns>
    public static IHtmlContent Format<T>(this ILocalizer localizer, string key, params T[] args)
    {
        if (localizer == null)
            throw new ArgumentNullException(nameof(localizer));
        if (key == null)
            throw new ArgumentNullException(nameof(key));
        if (args == null || !args.Any())
            throw new ArgumentNullException(nameof(key));

        var tagBuilders = new List<string>();
        foreach (var arg in args)
        {
            if (arg is Func<string, IHtmlContent> tag)
            {
                var htmlTag = tag.Invoke("").GetString();
                tagBuilders.Add(htmlTag);
            }
            else
            {
                tagBuilders.Add(arg.ToString());
            }
        }

        var localized = localizer[key];

        if (tagBuilders.Count == 0)
            return new HtmlString(localized);

        var formattedString = string.Format(localized, tagBuilders.ToArray());
        return new HtmlString(formattedString);
    }

    /// <summary>
    /// Returns a <see cref="IHtmlContent"/> object that representing formatted input.
    /// </summary>
    /// <param name="localizer">An instance of <see cref="ILocalizer"/>.</param>
    /// <param name="key">Name of specific key in <see cref="ILocalizer.Dictionary"/> that containing a localized text.</param>
    /// <param name="tags">A collection of html tags that should be replaced in given text.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>A <see cref="IHtmlContent"/> object that representing a formatted text with given tags.</returns>
    public static IHtmlContent Format(this ILocalizer localizer, string key, params Func<string, IHtmlContent>[] tags)
    {
        if (localizer == null)
            throw new ArgumentNullException(nameof(localizer));
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        return localizer.Format<Func<string, IHtmlContent>>(key, tags);
    }

    /// <summary>
    /// Deserializes already-serialized JSON to new <see cref="LocalizerValueCollection"/> instance.
    /// </summary>
    /// <param name="json">A <see cref="string"/> json.</param>
    /// <returns>A <see cref="LocalizerValueCollection"/> instance.</returns>
    public static LocalizerValueCollection Deserialize(string json)
    {
        var output = (LocalizerValueCollection)null;
        if (string.IsNullOrEmpty(json) || string.IsNullOrWhiteSpace(json))
            return null;

        json = json.Trim();
        if (json.StartsWith("{") &&
            json.EndsWith("}") ||
            json.StartsWith("[") &&
            json.EndsWith("]"))
        {
            output = JsonConvert.DeserializeObject<LocalizerValueCollection>(json, SerializerSettings);
        }

        return output;
    }

    private static JsonSerializerSettings SerializerSettings
    {
        get
        {
            var settings = new JsonSerializerSettings
            {
                Error = (serializer, err) => err.ErrorContext.Handled = true,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                Formatting = Formatting.None,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
            };

            settings.Converters.Insert(0, new ContainerJsonConverter());
            return settings;
        }
    }

    internal class ContainerJsonConverter : JsonConverter<LocalizerValueCollection>
    {
        public override void WriteJson(JsonWriter writer, LocalizerValueCollection value, JsonSerializer serializer)
        {
            var jObject = new JObject();
            if (value.Keys.Any())
            {
                var keys = value.Keys
                    .OrderBy(x => x.TwoLetterISOLanguageName)
                    .GetEnumerator();
                var i = 0;
                keys.MoveNext();
                var culture = keys.Current;
                while (culture != null)
                {
                    value.TryGetValue(culture, out var s);
                    jObject.Add(culture.TwoLetterISOLanguageName, s);
                    i++;

                    if (keys.MoveNext())
                        culture = keys.Current;
                }
            }

            jObject.WriteTo(writer);
        }

        public override LocalizerValueCollection ReadJson(JsonReader reader, Type objectType, LocalizerValueCollection existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return new LocalizerValueCollection();

            var obj = new LocalizerValueCollection();
            var jToken = JToken.Load(reader);
            if (jToken == null || !jToken.Any())
                return new LocalizerValueCollection();

            var token = jToken.First;
            while (token != null)
            {
                var property = token.ToObject<JProperty>();
                var culture = CultureInfo.GetCultureInfo(property.Name);
                var localized = property.Value.ToString();
                obj.Add(culture, localized);

                token = token.Next;
            }

            return obj;
        }
    }

    /// <summary>
    /// Serializes current instance to JSON string in specific format.
    /// </summary>
    /// <returns>A <see cref="string"/> json value.</returns>
    public static string Serialize(this LocalizerValueCollection LocalizerValueCollection)
    {
        return LocalizerValueCollection.Keys.Count <= 0
            ? null
            : JsonConvert.SerializeObject(LocalizerValueCollection, SerializerSettings);
    }
}