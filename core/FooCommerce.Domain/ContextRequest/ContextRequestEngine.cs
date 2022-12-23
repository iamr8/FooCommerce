using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FooCommerce.Domain.ContextRequest;

public record ContextRequestEngine
{
    [Required, JsonRequired, RegularExpression("^[A-Z0-9]{1}[a-zA-Z0-9 -]{1,30}")]
    public string Name { get; init; }
}