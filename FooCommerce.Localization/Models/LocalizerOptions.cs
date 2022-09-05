using System.Data;

namespace FooCommerce.Localization.Models;

public record LocalizerOptions
{
    public Func<IDbConnection, Task<LocalizerDictionary>> Provider { get; set; }
}