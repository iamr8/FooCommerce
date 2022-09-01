using System.Text.Json;
using System.Text.Json.Nodes;

using FooCommerce.Application.Localization.Models;
using FooCommerce.Core.Configurations;

namespace FooCommerce.Core.Localization.Helpers;

public static class LocalizerSerializationHelper
{
    /// <summary>
    /// Deserializes a <see cref="JsonNode"/> object to a new <see cref="LocalizerValueCollection"/> instance.
    /// </summary>
    /// <param name="node">A <see cref="JsonNode"/> json.</param>
    /// <returns>A <see cref="LocalizerValueCollection"/> instance.</returns>
    public static LocalizerValueCollection Deserialize(JsonNode node)
    {
        var output = node.Deserialize<LocalizerValueCollection>(JsonDefaultSettings.Settings);

        return output;
    }

    /// <summary>
    /// Deserializes already-serialized JSON to a new <see cref="LocalizerValueCollection"/> instance.
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
            output = JsonSerializer.Deserialize<LocalizerValueCollection>(json, JsonDefaultSettings.Settings);
        }

        return output;
    }

    /// <summary>
    /// Serializes current instance to JSON string in specific format.
    /// </summary>
    /// <returns>A <see cref="string"/> json value.</returns>
    public static string Serialize(this LocalizerValueCollection LocalizerValueCollection)
    {
        return LocalizerValueCollection.Keys.Count <= 0
            ? null
            : JsonSerializer.Serialize(LocalizerValueCollection, JsonDefaultSettings.Settings);
    }
}