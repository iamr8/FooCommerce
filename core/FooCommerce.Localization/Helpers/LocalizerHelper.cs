using FooCommerce.Localization.Attributes;

namespace FooCommerce.Localization.Helpers;

public static class LocalizerHelper
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="en"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static string GetLocalizer<T>(this T en) where T : struct, Enum
    {
        var enumType = en.GetType();
        var enumName = Enum.GetName(enumType, en);

        var localizerAttribute = enumType
            .GetField(enumName)
            .GetCustomAttributes(typeof(LocalizerAttribute), false)
            .FirstOrDefault() as LocalizerAttribute;
        if (localizerAttribute == null)
            throw new Exception($"Localizer attribute not found for {enumName} enum value.");

        return localizerAttribute.Key;
    }
}