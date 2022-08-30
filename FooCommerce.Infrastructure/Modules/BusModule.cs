using Autofac;

using FooCommerce.Application.Helpers;
using FooCommerce.Core.JsonCustomization;
using FooCommerce.Core.Modules;

using MassTransit;

namespace FooCommerce.Infrastructure.Modules;

public class BusModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var moduleAssemblies = AppDomain.CurrentDomain.GetSolutionAssemblies().ToArray();
        var moduleConfigurations = moduleAssemblies
            .SelectMany(x => x.DefinedTypes)
            .Where(t => t.GetInterfaces().Any(c => c == typeof(IModuleConfiguration)))
            .Select(x => (IModuleConfiguration)Activator.CreateInstance(x))
            .ToList();

        builder.AddMassTransit(cfg =>
        {
            var entryAssembly = this.GetType().Assembly;
            cfg.SetKebabCaseEndpointNameFormatter();
            cfg.SetInMemorySagaRepositoryProvider();
            cfg.AddMediator();

            foreach (var config in moduleConfigurations)
                config.AddConsumers(cfg);

            cfg.AddConsumers(this.GetType().Assembly);

            cfg.UsingRabbitMq((context, config) =>
            {
                config.AutoStart = true;
                config.Host("localhost", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                config.ConfigureJsonSerializerOptions(options =>
                {
                    options.Converters.Clear();
                    foreach (var jsonConverter in DefaultSettings.Settings.Converters)
                        options.Converters.Add(jsonConverter);

                    options.DefaultIgnoreCondition = DefaultSettings.Settings.DefaultIgnoreCondition;
                    options.UnknownTypeHandling = DefaultSettings.Settings.UnknownTypeHandling;
                    options.DictionaryKeyPolicy = DefaultSettings.Settings.DictionaryKeyPolicy;
                    options.PropertyNameCaseInsensitive = DefaultSettings.Settings.PropertyNameCaseInsensitive;
                    options.PropertyNamingPolicy = DefaultSettings.Settings.PropertyNamingPolicy;
                    options.WriteIndented = true;
                    return options;
                });

                config.ConfigureEndpoints(context);
            });
        });

        foreach (var config in moduleConfigurations)
            config.RegisterServices(builder);
    }
}