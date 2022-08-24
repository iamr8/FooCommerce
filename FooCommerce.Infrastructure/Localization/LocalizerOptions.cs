namespace FooCommerce.Infrastructure.Localization;

public record LocalizerOptions
{
    public Func<Task<LocalizerDictionary>> Provider { get; set; }
}