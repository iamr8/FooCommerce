using Autofac;
using Autofac.Extensions.DependencyInjection;

using FooCommerce.Core.JsonCustomization;

using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FooCommerce.NotificationAPI.Modules;

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
                foreach (var jsonConverter in DefaultSettings.Settings.Converters)
                    options.JsonSerializerOptions.Converters.Add(jsonConverter);

                options.JsonSerializerOptions.DefaultIgnoreCondition = DefaultSettings.Settings.DefaultIgnoreCondition;
                options.JsonSerializerOptions.UnknownTypeHandling = DefaultSettings.Settings.UnknownTypeHandling;
                options.JsonSerializerOptions.DictionaryKeyPolicy = DefaultSettings.Settings.DictionaryKeyPolicy;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = DefaultSettings.Settings.PropertyNameCaseInsensitive;
                options.JsonSerializerOptions.PropertyNamingPolicy = DefaultSettings.Settings.PropertyNamingPolicy;
                options.JsonSerializerOptions.WriteIndented = true;
            });

        builder.Populate(services);
    }
}