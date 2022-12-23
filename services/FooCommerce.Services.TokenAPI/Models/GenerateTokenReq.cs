using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FooCommerce.TokenService.Models;

[Serializable]
public record GenerateTokenReq
{
    /// <summary>
    /// A Unique identifier to generate a token for.
    /// </summary>
    [Required, JsonRequired, JsonPropertyName("id")]
    public Guid Identifier { get; set; }

    /// <summary>
    /// Token's lifetime in Seconds.
    /// </summary>
    [Required, JsonRequired, JsonPropertyName("interval")]
    public long LifetimeInSeconds { get; set; }
}

[Serializable]
public record GenerateTokenResp
{
    /// <summary>
    /// A Unique Identifier for the generated token instance.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid TokenId { get; set; }
    /// <summary>
    /// The expiry date of the token in Unix second time.
    /// </summary>
    [JsonPropertyName("expiry")]
    public long Expiry { get; set; }
}