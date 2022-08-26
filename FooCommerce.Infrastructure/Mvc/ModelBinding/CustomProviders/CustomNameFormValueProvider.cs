using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

namespace FooCommerce.Infrastructure.Mvc.ModelBinding.CustomProviders;

/// <summary>
/// An <see cref="IValueProvider"/> adapter for data stored in an <see cref="IFormCollection"/>.
/// </summary>
public class CustomNameFormValueProvider : BindingSourceValueProvider, IEnumerableValueProvider
{
    private readonly IFormCollection _values;
    private PrefixContainer _prefixContainer;
    private string _prefix;

    /// <summary>
    /// Creates a value provider for <see cref="IFormCollection"/>.
    /// </summary>
    /// <param name="bindingSource">The <see cref="BindingSource"/> for the data.</param>
    /// <param name="values">The key value pairs to wrap.</param>
    /// <param name="culture">The culture to return with ValueProviderResult instances.</param>
    public CustomNameFormValueProvider(
        BindingSource bindingSource,
        IFormCollection values,
        CultureInfo culture)
        : base(bindingSource)
    {
        if (bindingSource == null)
        {
            throw new ArgumentNullException(nameof(bindingSource));
        }

        if (values == null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        var dic = new Dictionary<string, StringValues>();
        foreach (var (key, value) in values)
        {
            var _key = key;
            if (key.Contains('['))
            {
                string prefix;
                var s = key.IndexOf('[');
                var e = key.LastIndexOf(']', s + 1);
                if (e == -1)
                    e = key.IndexOf(']', s + 1);

                var t = key[(s + 1)..e];
                if (t.Contains('['))
                {
                    // cutomized array
                    var s2 = t.IndexOf('[');
                    prefix = key[..(s + s2 + 1)].Replace("[", ".");
                    _key = $"{prefix}{key[(s + s2 + 1)..^1]}";
                }
                else
                {
                    prefix = key[..s];
                    if (int.TryParse(t, out _))
                    {
                        // builtin array
                        _key = $"{prefix}{key[s..]}";
                    }
                    else
                    {
                        // customized name
                        _key = $"{prefix}.{key[(s + 1)..^1]}";

                        var hasBuiltinDuplicate = dic.Any(x => x.Key.Equals(_key, StringComparison.InvariantCultureIgnoreCase));
                        if (hasBuiltinDuplicate)
                        {
                            var builtinDuplicate = dic.First(x => x.Key.Equals(_key, StringComparison.InvariantCultureIgnoreCase));
                            dic.Remove(builtinDuplicate.Key);
                        }
                    }
                }
            }
            else
            {
                // builtin name
                var hasBuiltinDuplicate = dic.Any(x => x.Key.Equals(_key, StringComparison.InvariantCultureIgnoreCase));
                if (hasBuiltinDuplicate)
                    continue;
            }

            dic.Add(_key, value);
        }

        _values = new FormCollection(dic);
        Culture = culture;
    }

    /// <summary>
    /// The culture to use.
    /// </summary>
    public CultureInfo Culture { get; }

    /// <summary>
    /// The prefix container.
    /// </summary>
    protected PrefixContainer PrefixContainer
    {
        get
        {
            if (_prefixContainer == null)
            {
                _prefixContainer = new PrefixContainer(_values.Keys);
            }

            return _prefixContainer;
        }
    }

    /// <inheritdoc />
    public override bool ContainsPrefix(string prefix)
    {
        _prefix = prefix;
        return PrefixContainer.ContainsPrefix(prefix);
    }

    /// <inheritdoc />
    public virtual IDictionary<string, string> GetKeysFromPrefix(string prefix)
    {
        if (prefix == null)
        {
            throw new ArgumentNullException(nameof(prefix));
        }

        _prefix = prefix;
        return PrefixContainer.GetKeysFromPrefix(prefix);
    }

    /// <inheritdoc />
    public override ValueProviderResult GetValue(string key)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (key.Length == 0)
        {
            // Top level parameters will fall back to an empty prefix when the parameter name does not
            // appear in any value provider. This would result in the parameter binding to a form parameter
            // with a empty key (e.g. Request body looks like "=test") which isn't a scenario we want to support.
            // Return a "None" result in this event.
            return ValueProviderResult.None;
        }

        // Add more abilities to find out new input names, according to InputCustomNameTagHelper

        var values = _values[key];
        if (values.Count == 0)
            values = _values.FirstOrDefault(x => x.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)).Value;

        if (values.Count == 0)
        {
            return ValueProviderResult.None;
        }
        else
        {
            return new ValueProviderResult(values, Culture);
        }
    }
}