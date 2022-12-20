using System.Text.Json.Serialization;

namespace FooCommerce.Services.TokenAPI.Models;

[Serializable]
public record GenerateTokenReq
{
    [JsonRequired, JsonPropertyName("id")]
    public Guid Identifier { get; set; }

    [JsonRequired, JsonPropertyName("interval")]
    public TimeSpan Interval { get; set; }
}

[Serializable]
public record GenerateTokenResp
{
    [JsonPropertyName("expiry")]
    public DateTime Expiry { get; set; }
}