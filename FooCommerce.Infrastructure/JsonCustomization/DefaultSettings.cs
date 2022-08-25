using System.Text.Json;
using System.Text.Json.Serialization;

using FooCommerce.Application.JsonConverters;
using FooCommerce.Infrastructure.JsonCustomization.Converters;

namespace FooCommerce.Infrastructure.JsonCustomization;

public static class DefaultSettings
{
    /// <summary>
    /// A custom-defined <see cref="JsonSerializerOptions"/>
    /// </summary>
    public static JsonSerializerOptions Settings
    {
        get
        {
            var settings = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            };

            settings.Converters.Add(new JsonCultureToStringConverter());
            settings.Converters.Add(new JsonGuidToStringConverter());
            settings.Converters.Add(new JsonIPAddressToStringConverter());
            settings.Converters.Add(new JsonRegionInfoToStringConverter());
            settings.Converters.Add(new JsonDateTimeZoneToStringConverter());
            settings.Converters.Add(new JsonHtmlContentToStringConverter());
            settings.Converters.Insert(0, new JsonDateTimeToUnixConverter());
            return settings;
        }
    }
}