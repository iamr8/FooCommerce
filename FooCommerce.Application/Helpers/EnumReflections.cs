using System.Reflection;

using FooCommerce.Application.Attributes;

namespace FooCommerce.Application.Helpers
{
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

        /// <summary>
        /// Creates an array from given <see cref="Enum{T}"/>
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>An array that contains the elements from the input sequence.</returns>
        public static int[] ToArray(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (!type.IsEnum)
                throw new ArgumentException(nameof(type));

            var array = Enum
                .GetValues(type)
                .Cast<int>()
                .ToArray();

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

            var hasOrderAttribute = fields.Any(x => x.GetCustomAttribute<OrderAttribute>() != null);
            if (!hasOrderAttribute)
                return array;

            var dictionary = new Dictionary<int, int?>();
            foreach (var fieldInfo in fields)
            {
                var order = fieldInfo.GetCustomAttribute<OrderAttribute>()?.Priority;
                var valid = Enum.TryParse(type, fieldInfo.Name, false, out var @enum);
                if (!valid || @enum == null)
                    continue;

                dictionary.Add((int)@enum, order);
            }

            var arr = new List<int>();
            var unordered = new List<int>();
            var ordered = new Dictionary<int, int>();
            foreach (var (key, order) in dictionary)
            {
                if (order != null)
                    ordered.Add(key, order.Value);
                else
                    unordered.Add(key);
            }

            if (ordered?.Any() == true)
                arr.AddRange(ordered.OrderBy(x => x.Value).Select(x => x.Key));

            if (unordered?.Any() == true)
                arr.AddRange(unordered);

            return arr.ToArray();
        }
    }
}