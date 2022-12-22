using System.Data;

namespace FooCommerce.Localization.Models;

public record LocalizerOptions
{
    public Func<Task<LocalizerDictionary>> Provider { get; set; }
}