using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FooCommerce.Services.TokenAPI.Models;

[Serializable]
public record ValidateTokenReq
{
    /// <summary>
    /// A Unique identifier for the token that is already generated.
    /// </summary>
    [Required, JsonRequired, JsonPropertyName("id")]
    public Guid TokenId { get; set; }

    /// <summary>
    /// A code to validate the generated token.
    /// </summary>
    [Required, JsonRequired, JsonPropertyName("code"), RegularExpression("\\d{5}")]
    public string Code { get; set; }
}

[Serializable]
public record ValidateTokenResp
{
}