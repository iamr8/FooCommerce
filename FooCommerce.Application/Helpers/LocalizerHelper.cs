using FooCommerce.Application.Attributes;

namespace FooCommerce.Application.Helpers;

public static class LocalizerHelper
{
    /// <summary>
    /// Returns stored Localizer key.
    /// </summary>
    /// <typeparam name="T">Type of enum</typeparam>
    /// <param name="enum">enum value.</param>
    /// <returns>A Localizer key.</returns>
    public static string GetLocalizerKey<T>(this T @enum) where T : Enum
    {
        var fallback = @enum.ToString();

        var attribute = @enum.GetAttribute<LocalizerAttribute>();
        return attribute == null ? fallback : attribute.Key;
    }
}