using System.Globalization;

namespace FooCommerce.Application.Models.Localization;

public class LocalizerValueCollection : SortedList<CultureInfo, string>
{
    public LocalizerValueCollection(IDictionary<CultureInfo, string> dictionary) : base(dictionary)
    {
    }

    public LocalizerValueCollection()
    {
    }

    /// <summary>Gets or sets the value associated with the specified key.</summary>
    /// <param name="culture">The key whose value to get or set.</param>
    /// <exception cref="KeyNotFoundException">The property is retrieved and <paramref name="culture" /> does not exist in the collection.</exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="CultureNotFoundException"></exception>
    /// <returns>The value associated with the specified key. If the specified key is not found, a get operation throws a <see cref="T:System.Collections.Generic.KeyNotFoundException" /> and a set operation creates a new element using the specified key.</returns>
    public string this[string culture]
    {
        get
        {
            var cultureInfo = CultureInfo.GetCultureInfo(culture);
            return this[cultureInfo];
        }
    }

    public override string ToString()
    {
        return this[CultureInfo.CurrentCulture];
    }

    public static explicit operator string(LocalizerValueCollection value)
    {
        return value[CultureInfo.CurrentCulture];
    }
}