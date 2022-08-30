using System.Collections;

using FooCommerce.Application.Helpers;

using Microsoft.Extensions.Primitives;

namespace FooCommerce.Core.Helpers;

public static class InputHelper
{
    public static string GetWritableValueForDatabase(this StringValues values, Type propertyType)
    {
        if (!values.Any())
            return null;

        if (typeof(IEnumerable).IsAssignableFrom(propertyType))
            return string.Join(Constants.ValueDelimiter, values.ToArray());

        return values.ToString();
    }

    public static StringValues GetWritableStringValues(this Type propertyType, object value)
    {
        if (value == null)
            return new StringValues();

        if (propertyType == typeof(string))
            return new StringValues(value.ToString());

        var underlyingType = propertyType.GetUnderlyingType();
        if (typeof(IEnumerable).IsAssignableFrom(propertyType) && value is IEnumerable list)
        {
            var values = new List<object>();
            var objects = from object? item in list select item;
            if (objects?.Any() == true)
            {
                foreach (var obj in objects)
                {
                    if (obj == null)
                        continue;

                    var _value = underlyingType.GetWritableStringValues(obj);
                    values.Add(_value.ToString());
                }

                if (values?.Any() == true)
                    return new StringValues(values.Select(x => x.ToString()).ToArray());
            }
        }
        else
        {
            if (underlyingType.IsEnum)
            {
                var validEnum = Enum.TryParse(underlyingType, value.ToString(), true, out var _enum);
                if (validEnum)
                {
                    var _value = (int)_enum;
                    return new StringValues(_value.ToString());
                }
            }
            else if (underlyingType == typeof(bool))
            {
                var validBool = bool.TryParse(value.ToString(), out var _bool);
                if (validBool)
                {
                    var _value = _bool ? "1" : "0";
                    return new StringValues(_value);
                }
            }
            //else
            //{
            //    if (underlyingType == typeof(double))
            //    {
            //        double.TryParse(input.Price, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out var amount)
            //    }
            //}
        }

        return new StringValues(value.ToString());
    }
}