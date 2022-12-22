using FooCommerce.Localization;
using FooCommerce.Localization.Models;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Tests.Mocks;

public static class LocalizerMoqInjectionExtensions
{
    public static IServiceCollection AddLocalizerTesting(this IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            return new LocalizerOptions
            {
                Provider = () => Task.FromResult(new LocalizerDictionary(new Dictionary<string, LocalizerValueCollection>()))
            };
        });
        services.AddSingleton<ILocalizer, Localizer>();

        return services;
    }
}