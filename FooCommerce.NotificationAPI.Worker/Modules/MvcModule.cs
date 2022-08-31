using Autofac;
using Autofac.Extensions.DependencyInjection;
using FooCommerce.Core;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FooCommerce.NotificationAPI.Worker.Modules;

public class MvcModule : Module
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly Action<RazorPagesOptions> _razorOptions;

    public MvcModule(IWebHostEnvironment webHostEnvironment, Action<RazorPagesOptions> razorOptions = null)
    {
        _webHostEnvironment = webHostEnvironment;
        _razorOptions = razorOptions;
    }

    protected override void Load(ContainerBuilder builder)
    {
        var services = new ServiceCollection();

        services.Configure<ForwardedHeadersOptions>(options =>
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);

        services
            .AddMvc()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Clear();
                foreach (var jsonConverter in JsonDefaultSettings.Settings.Converters)
                    options.JsonSerializerOptions.Converters.Add(jsonConverter);

                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonDefaultSettings.Settings.DefaultIgnoreCondition;
                options.JsonSerializerOptions.UnknownTypeHandling = JsonDefaultSettings.Settings.UnknownTypeHandling;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonDefaultSettings.Settings.DictionaryKeyPolicy;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = JsonDefaultSettings.Settings.PropertyNameCaseInsensitive;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonDefaultSettings.Settings.PropertyNamingPolicy;
                options.JsonSerializerOptions.WriteIndented = true;
            });

        builder.Populate(services);
    }
}