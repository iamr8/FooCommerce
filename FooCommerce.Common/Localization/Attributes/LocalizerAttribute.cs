namespace FooCommerce.Common.Localization.Attributes;

/// <summary>
/// Stores a Localizer Key for current property/field.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class LocalizerAttribute : Attribute
{
    public readonly string Key;

    public LocalizerAttribute(string key)
    {
        Key = key;
    }
}