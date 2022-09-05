﻿using System.Text.Json;
using System.Text.Json.Serialization;
using FooCommerce.Common.JsonConverters;

namespace FooCommerce.Common.Configurations;

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

            settings.Converters.Add(new JsonCultureToStringConverter());
            settings.Converters.Add(new JsonGuidToStringConverter());
            settings.Converters.Add(new JsonIPAddressToStringConverter());
            settings.Converters.Add(new JsonIPEndPointConverter());
            settings.Converters.Add(new JsonRegionInfoToStringConverter());
            settings.Converters.Insert(0, new JsonDateTimeToUnixConverter());
            return settings;
        }
    }
}