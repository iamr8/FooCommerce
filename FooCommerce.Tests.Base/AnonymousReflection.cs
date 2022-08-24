using System.Reflection;

namespace FooCommerce.Tests.Base;

public static class AnonymousReflection
{
    /// <summary>
    /// Returns a <see cref="Dictionary{TKey,TValue}"/> from properties in given source.
    /// </summary>
    /// <typeparam name="TModel">A generic type for source.</typeparam>
    /// <param name="source">An object to get properties list.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>A <see cref="Dictionary{TKey,TValue}"/> object</returns>
    public static Dictionary<string, object> ToDictionary<TModel>(TModel source) where TModel : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        var dictionary = new Dictionary<string, object>();
        foreach (var property in source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var value = property.GetValue(source);
            if (value is DateTime)
                value = $"{value:dd/MM/yyyy}";

            dictionary.Add(property.Name, value);
        }
        return dictionary;
    }
}