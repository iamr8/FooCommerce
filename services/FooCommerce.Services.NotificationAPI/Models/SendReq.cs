using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FooCommerce.Domain.ContextRequest;
using FooCommerce.Domain.Enums;
using FooCommerce.NotificationService.Enums;

namespace FooCommerce.NotificationService.Models;

[Serializable]
[JsonSerializable(typeof(ClientInfo))]
public record SendReq
{
    [JsonRequired, JsonPropertyName("purpose")]
    public NotificationPurpose Purpose { get; init; }

    [JsonRequired, JsonPropertyName("receiver")]
    public string ReceiverName { get; set; }
    [JsonRequired, JsonPropertyName("receiverAddresses")]
    public ReceiverCommunications ReceiverCommunications { get; set; } = new();
    [JsonRequired, JsonPropertyName("url"), Url, RegularExpression("^(http(s)?)\\:\\/\\/([a-zA-Z0-9]{1,20}\\.)?([a-zA-Z0-9.]{1,20})\\.([a-zA-Z0-9]{1,10})")]
    public string BaseUrl { get; set; }

    [JsonPropertyName("links")]
    public Dictionary<string, string> Links { get; set; } = new();

    [JsonPropertyName("formatters")]
    public Dictionary<string, string> Formatters { get; set; } = new();

    [JsonRequired, JsonPropertyName("headers")]
    public ClientInfo Headers { get; set; }
}

[Serializable]
public record ReceiverCommunications
{
    [EmailAddress]
    public string Email { get; set; }
    [Phone, RegularExpression("\\+[\\d]{5,15}")]
    public string Phone { get; set; }
    public string PushId { get; set; }

    public Dictionary<CommType, string> ToDictionary()
    {
        var comms = new Dictionary<CommType, string>();
        if (this.Email != null)
            comms.Add(CommType.Email, this.Email);
        if (this.Phone != null)
            comms.Add(CommType.Sms, this.Phone);
        if (this.PushId != null)
            comms.Add(CommType.Push, this.PushId);
        return comms;
    }
}
[Serializable]
[JsonSerializable(typeof(ContextRequestBrowser))]
[JsonSerializable(typeof(ContextRequestEngine))]
[JsonSerializable(typeof(ContextRequestPlatform))]
[JsonSerializable(typeof(ContextRequestDevice))]
public record ClientInfo
{
    [JsonRequired, JsonPropertyName("ip"), RegularExpression("^([0-9]{1,3})\\.([0-9]{1,3})\\.([0-9]{1,3})\\.([0-9]{1,3})$")]
    public string IPAddress { get; set; }

    [JsonRequired, JsonPropertyName("ua")]
    public string? UserAgent { get; set; }

    [JsonRequired, JsonPropertyName("country"), RegularExpression("^[A-Z]{2,3}")]
    public string Country { get; set; }

    [JsonRequired, JsonPropertyName("culture"), RegularExpression("^[a-z]{2}(-[A-Z]{2})?")]
    public string Culture { get; set; }

    [JsonRequired, JsonPropertyName("timezone"), RegularExpression("^[a-zA-Z]{2,15}\\/[a-zA-Z]{2,15}")]
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