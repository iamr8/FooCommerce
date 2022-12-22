using System.Globalization;
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
    [JsonRequired, JsonPropertyName("comms")]
    public IDictionary<CommType, string> ReceiverCommunications { get; set; } = new Dictionary<CommType, string>();
    [JsonRequired, JsonPropertyName("url")]
    public string BaseUrl { get; set; }

    [JsonPropertyName("links")]
    public IDictionary<string, string> Links { get; set; } = new Dictionary<string, string>();

    [JsonPropertyName("formatters")]
    public IDictionary<string, string> Formatters { get; set; } = new Dictionary<string, string>();

    [JsonRequired, JsonPropertyName("requestInfo")]
    public ContextRequestInfo RequestInfo { get; set; }
}

[Serializable]
public record SendResp
{
}