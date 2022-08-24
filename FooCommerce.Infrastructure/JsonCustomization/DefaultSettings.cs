using System.Globalization;

using FooCommerce.Infrastructure.JsonCustomization.Contracts;
using FooCommerce.Infrastructure.JsonCustomization.Converters;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FooCommerce.Infrastructure.JsonCustomization;

public static class DefaultSettings
{
    /// <summary>
    /// A custom-defined <see cref="JsonSerializerSettings"/>
    /// </summary>
    public static JsonSerializerSettings Settings
    {
        get
        {
            var settings = new JsonSerializerSettings()
            {
                Error = (serializer, err) => err.ErrorContext.Handled = true,
                DefaultValueHandling = DefaultValueHandling.Populate,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                Formatting = Formatting.None,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                TypeNameHandling = TypeNameHandling.Auto,
                ContractResolver = new NullToEmptyContractResolver(),
                Culture = new CultureInfo("en-US"),
            };

            settings.Converters.Add(new JsonCultureConverter());
            settings.Converters.Add(new JsonDateTimeToUnixConverter());
            settings.Converters.Add(new JsonGeometryConverter());
            settings.Converters.Add(new JsonGuidConverter());
            settings.Converters.Add(new JsonIPAddressConverter());
            settings.Converters.Add(new JsonHtmlContentConverter());

            settings.Converters.Insert(0, new UnixDateTimeConverter());
            return settings;
        }
    }
}