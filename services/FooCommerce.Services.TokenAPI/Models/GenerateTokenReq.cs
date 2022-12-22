using System.Text.Json.Serialization;

using FooCommerce.Domain.Jsons.JsonConverters;

namespace FooCommerce.Services.TokenAPI.Models;

[Serializable]
public record GenerateTokenReq
{
    [JsonRequired, JsonPropertyName("id"), JsonConverter(typeof(JsonGuidToStringConverter))]
    public Guid Identifier { get; set; }

    [JsonRequired, JsonPropertyName("interval")]
    public long Interval { get; set; }
}

[Serializable]
public record GenerateTokenResp
{
    [JsonPropertyName("expiry"), JsonConverter(typeof(JsonDateTimeToUnixConverter))]
    public DateTime Expiry { get; set; }
}