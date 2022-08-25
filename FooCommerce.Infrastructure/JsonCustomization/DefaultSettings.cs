using System.Globalization;

using FooCommerce.Application.JsonConverters;
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
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
            };

            settings.Converters.Add(new JsonCultureToStringConverter());
            settings.Converters.Add(new JsonGeometryToArrayDoubleConverter());
            settings.Converters.Add(new JsonGuidToStringConverter());
            settings.Converters.Add(new JsonIPAddressToStringConverter());
            settings.Converters.Add(new JsonRegionInfoToStringConverter());
            settings.Converters.Add(new JsonDateTimeZoneToStringConverter());
            settings.Converters.Add(new JsonHtmlContentToStringConverter());

            settings.Converters.Insert(0, new UnixDateTimeConverter());
            return settings;
        }
    }
}