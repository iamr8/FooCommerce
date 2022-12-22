using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FooCommerce.Services.MembershipAPI.Models;

[Serializable]
public record RegisterReq
{
    [JsonRequired, JsonPropertyName("firstName")]
    public string FirstName { get; init; }
    [JsonRequired, JsonPropertyName("lastName")]
    public string LastName { get; init; }
    [JsonRequired, JsonPropertyName("email")]
    [EmailAddress]
    public string Email { get; init; }
    [JsonRequired, JsonPropertyName("password")]
    public string Password { get; init; }
    //[JsonRequired, JsonPropertyName("country"), StringLength(2, MinimumLength = 2)]
    //public uint Country { get; init; }
}

[Serializable]
public record RegisterResp
{
    [JsonPropertyName("commId")]
    public Guid? CommunicationId { get; init; }
}