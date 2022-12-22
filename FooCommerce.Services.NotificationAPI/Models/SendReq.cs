using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using FooCommerce.Domain.ContextRequest;
using FooCommerce.Domain.Enums;
using FooCommerce.Services.NotificationAPI.Enums;

namespace FooCommerce.Services.NotificationAPI.Models;

[Serializable]
public record SendReq
{
    [JsonRequired, JsonPropertyName("purpose")]
    public NotificationPurpose Purpose { get; init; }

    [JsonRequired, JsonPropertyName("receiver")]
    public string ReceiverName { get; set; }
    [JsonRequired, JsonPropertyName("receiverAddresses")]
    public Dictionary<CommType, string> ReceiverCommunications { get; set; } = new();
    [JsonRequired, JsonPropertyName("url")]
    public string BaseUrl { get; set; }

    [JsonPropertyName("links")]
    public Dictionary<string, string> Links { get; set; } = new();

    [JsonPropertyName("formatters")]
    public Dictionary<string, string> Formatters { get; set; } = new();

    [JsonRequired, JsonPropertyName("requestInfo")]
    public ClientInfo RequestInfo { get; set; }
}

[Serializable]
public record ClientInfo
{
    [JsonRequired, JsonPropertyName("ip")]
    public string IPAddress { get; set; }

    [JsonRequired, JsonPropertyName("ua")]
    public string? UserAgent { get; set; }

    [JsonRequired, JsonPropertyName("country"), StringLength(2, MinimumLength = 2)]
    public string Country { get; set; }

    [JsonRequired, JsonPropertyName("culture"), StringLength(5, MinimumLength = 2)]
    public string Culture { get; set; }

    [JsonRequired, JsonPropertyName("timezone"), StringLength(50, MinimumLength = 5)]
    public string TimezoneId { get; set; }

    [JsonRequired, JsonPropertyName("browser")]
    public ContextRequestBrowser Browser { get; set; }
    [JsonRequired, JsonPropertyName("engine")]
    public ContextRequestEngine Engine { get; set; }

    [JsonRequired, JsonPropertyName("platform")]
    public ContextRequestPlatform Platform { get; set; }

    [JsonRequired, JsonPropertyName("device")]
    public ContextRequestDevice Device { get; set; }
}

[Serializable]
public record SendResp
{
}