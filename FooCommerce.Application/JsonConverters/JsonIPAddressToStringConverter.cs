﻿using System.Net;

using Newtonsoft.Json;

namespace FooCommerce.Application.JsonConverters;

public class JsonIPAddressToStringConverter : JsonConverter<IPAddress>
{
    public override void WriteJson(JsonWriter writer, IPAddress value, JsonSerializer serializer)
    {
        var plainIp = value == null || IPAddress.IsLoopback(value)
            ? null
            : value.ToString();
        writer.WriteValue(plainIp);
    }

    public override IPAddress ReadJson(JsonReader reader, Type objectType, IPAddress existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var ip = (string)reader.Value;

        return string.IsNullOrEmpty(ip)
            ? IPAddress.None
            : IPAddress.Parse(ip);
    }
}