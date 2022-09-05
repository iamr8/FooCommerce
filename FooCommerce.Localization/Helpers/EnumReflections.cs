using System.Reflection;

namespace FooCommerce.Localization.Helpers;

public static class EnumReflections
{
    /// <summary>
    /// Returns specific <see cref="Attribute"/> from given enum member.
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="enumMember"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    public static TAttribute GetAttribute<TAttribute>(this Enum enumMember)
        where TAttribute : Attribute
    {
        if (enumMember == null)
            throw new ArgumentNullException(nameof(enumMember));

        var enumType = enumMember.GetType();
        var enumName = Enum.GetName(enumType, enumMember);
        var memberInfos = enumType.GetMember(enumName);
        var memberInfo = memberInfos.Single();
        return memberInfo.GetCustomAttribute<TAttribute>();
    }
}