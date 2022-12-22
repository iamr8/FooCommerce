using System.Data;
using System.Globalization;
using System.Text.Json;

using Dapper;

using FooCommerce.Localization.Models;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Localization.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocalizer(this IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            return new LocalizerOptions
            {
                Provider = () =>
                {
                    var dbConnection = sp.GetService<IDbConnection>();
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
        });
        services.AddSingleton<ILocalizer, Localizer>();

        return services;
    }
}