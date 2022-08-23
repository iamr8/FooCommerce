using System.Globalization;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using Dapper;

using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Models.Localization;
using FooCommerce.Domain.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

namespace FooCommerce.Infrastructure.Modules;

public class LocalizationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var services = new ServiceCollection();

        services.AddSingleton(ctx =>
        {
            var options = new LocalizerOptions
            {
                Provider = () =>
                {
                    var dbConnectionFactory = ctx.GetService<IDbConnectionFactory>();
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
                        var __dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonValue);
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
        services.AddSingleton<ILocalizer, Localizer>();
        builder.Populate(services);
    }
}