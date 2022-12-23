using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FooCommerce.Domain.ContextRequest;

public record ContextRequestDevice
{
    [Required, JsonRequired, RegularExpression("^[A-Z0-9]{1}[a-zA-Z0-9 -]{1,30}")]
    public string Type { get; init; }
}