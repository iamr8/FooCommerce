namespace FooCommerce.Infrastructure.Localization.Models;

public record LocalizerOptions
{
    public Func<Task<LocalizerDictionary>> Provider { get; set; }
}