using System.Text.Json.Serialization;

using FooCommerce.Domain.Jsons.JsonConverters;
using FooCommerce.Services.TokenAPI.Enums;

namespace FooCommerce.Services.TokenAPI.Models;

[Serializable]
public record ValidateTokenReq
{
    [JsonRequired, JsonPropertyName("id"), JsonConverter(typeof(JsonGuidToStringConverter))]
    public Guid Identifier { get; set; }

    [JsonRequired, JsonPropertyName("code")]
    public string Code { get; set; }
}

[Serializable]
public record ValidateTokenResp
{
    [JsonPropertyName("status")]
    public TokenStatus Status { get; set; }
}