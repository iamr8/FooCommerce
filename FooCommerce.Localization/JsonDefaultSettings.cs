using System.Text.Json;
using System.Text.Json.Serialization;

namespace FooCommerce.Localization;

public static class JsonDefaultSettings
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
                WriteIndented = false,
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                UnknownTypeHandling = JsonUnknownTypeHandling.JsonNode,
            };

            return settings;
        }
    }
}