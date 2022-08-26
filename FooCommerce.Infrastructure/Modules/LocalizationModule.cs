using System.Globalization;
using System.Text.Json;

using Autofac;

using Dapper;
using FooCommerce.Application.DbProvider.Interfaces;
using FooCommerce.Application.Localization.Models;
using FooCommerce.Domain.Interfaces;
using FooCommerce.Infrastructure.Localization;
using FooCommerce.Infrastructure.Localization.Models;

namespace FooCommerce.Infrastructure.Modules;

public class LocalizationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(ctx =>
        {
            var options = new LocalizerOptions
            {
                Provider = () =>
                {
                    var dbConnectionFactory = ctx.Resolve<IDbConnectionFactory>();
                    using var dbConnection = dbConnectionFactory!.CreateConnection();
                    var values = dbConnection.QueryAsync($"SELECT [translation].Key, [translation].Value " +
                                                         $"FROM [Translations] AS [translation]" +
                                                         $"ORDER BY [translation].Created").GetAwaiter().GetResult().AsList();
                    if (values == null || !values.Any())
                        return null;

                    var dict = new Dictionary<string, LocalizerValueCollection>();
                    for (var i = 0; i < values.Count; i++)
                    {
                        var _value = values[i];
                        var key = (string)_value.Key;
                        var jsonValue = (string)_value.Value;
                        var __dict = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonValue);
                        if (__dict == null)
                            continue;

                        var _dict = __dict.ToDictionary(x => CultureInfo.GetCultureInfo(x.Key), x => x.Value);
                        dict.Add(key, new LocalizerValueCollection(_dict));
                    }

                    var output = new LocalizerDictionary(dict);
                    return Task.FromResult(output);
                }
            };
            return options;
        });
        builder.RegisterType<Localizer>()
            .As<ILocalizer>()
            .SingleInstance();
    }
}