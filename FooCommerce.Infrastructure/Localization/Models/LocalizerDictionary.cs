using System.Collections.ObjectModel;

using FooCommerce.Application.Models.Localization;

namespace FooCommerce.Infrastructure.Localization.Models;

public class LocalizerDictionary : ReadOnlyDictionary<string, LocalizerValueCollection>
{
    public LocalizerDictionary(IDictionary<string, LocalizerValueCollection> dictionary) : base(dictionary)
    {
    }

    public new LocalizerValueCollection this[string key] =>
        this.FirstOrDefault(x => x.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)).Value;
}