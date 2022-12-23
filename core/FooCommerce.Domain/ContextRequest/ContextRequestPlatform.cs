using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FooCommerce.Domain.ContextRequest;

[Serializable]
public record ContextRequestPlatform
{
    [Required, JsonRequired, RegularExpression("^[A-Z0-9]{1}[a-zA-Z0-9 -]{1,30}")]
    public string Name { get; init; }

    [Required, JsonRequired, RegularExpression("^[0-9a-zA-Z .]{1,10}")]
    public string Version { get; init; }
}