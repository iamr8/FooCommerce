using System.Text.Json.Nodes;

namespace FooCommerce.Domain.Helpers;

public static class JsonHelper
{
    public static bool TryGetGuid(this JsonObject json, string propertyName, out Guid value)
    {
        if (json.TryGetPropertyValue(propertyName, out var _value) &&
            _value is not null &&
            Guid.TryParse(_value.AsValue().ToString(), out var _guid) &&
            _guid != Guid.Empty)
        {
            value = _guid;
            return true;
        }

        value = Guid.Empty;
        return false;
    }
}