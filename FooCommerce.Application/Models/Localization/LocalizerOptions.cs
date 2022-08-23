namespace FooCommerce.Application.Models.Localization;

public record LocalizerOptions
{
    public Func<Task<LocalizerDictionary>> Provider { get; set; }
}