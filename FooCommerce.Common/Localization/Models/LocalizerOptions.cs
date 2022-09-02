using System.Data;

namespace FooCommerce.Common.Localization.Models;

public record LocalizerOptions
{
    public Func<IDbConnection, Task<LocalizerDictionary>> Provider { get; set; }
}