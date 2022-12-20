using System.Text.Json.Serialization;

using FooCommerce.Services.TokenAPI.Enums;

namespace FooCommerce.Services.TokenAPI.Models;

[Serializable]
public record ValidateTokenReq
{
    [JsonRequired, JsonPropertyName("id")]
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