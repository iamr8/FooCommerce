using System.Collections.ObjectModel;

namespace FooCommerce.Application.Models.Localization;

public class LocalizerDictionary : ReadOnlyDictionary<string, LocalizerValueCollection>
{
    public LocalizerDictionary(IDictionary<string, LocalizerValueCollection> dictionary) : base(dictionary)
    {
    }

    public new LocalizerValueCollection this[string key] =>
        this.FirstOrDefault(x => x.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)).Value;
}