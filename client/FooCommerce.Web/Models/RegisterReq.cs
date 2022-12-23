using System.Text.Json.Serialization;

namespace FooCommerce.Web.Models;

[JsonSerializable(typeof(RegisterReq))]
public record RegisterReq
{
    [JsonRequired, JsonPropertyName("firstName")]
    public string FirstName { get; init; }
    [JsonRequired, JsonPropertyName("lastName")]
    public string LastName { get; init; }
    [JsonRequired, JsonPropertyName("email")]
    public string Email { get; init; }
    [JsonRequired, JsonPropertyName("password")]
    public string Password { get; init; }
}

[JsonSerializable(typeof(RegisterResp))]
public record RegisterResp
{
}