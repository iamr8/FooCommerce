using Autofac;
using Autofac.Extensions.DependencyInjection;

using FooCommerce.Infrastructure.JsonCustomization;
using FooCommerce.Infrastructure.Mvc;
using FooCommerce.Infrastructure.Mvc.Localization;
using FooCommerce.Infrastructure.Mvc.ModelBinders;
using FooCommerce.Infrastructure.Mvc.ModelBinding.CustomProviders;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FooCommerce.Infrastructure.Modules;

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

        void AddMvcOptions(MvcOptions options)
        {
            if (options.ModelBinderProviders.All(x => x.GetType() != typeof(StringModelBinderProvider)))
                options.ModelBinderProviders.Insert(0, new StringModelBinderProvider());

            if (options.ModelBinderProviders.All(x => x.GetType() != typeof(DateTimeUtcModelBinderProvider)))
                options.ModelBinderProviders.Insert(0, new DateTimeUtcModelBinderProvider());

            if (options.ValueProviderFactories.All(x => x.GetType() != typeof(SeparatedQueryStringValueProviderFactory)))
                options.ValueProviderFactories.Insert(0, new SeparatedQueryStringValueProviderFactory());

            options.RespectBrowserAcceptHeader = true;

            // For supporting Input/Select/TextArea custom names
            if (options.ValueProviderFactories.All(x => x.GetType() != typeof(CustomNameFormValueProviderFactory)))
            {
                options.ValueProviderFactories.RemoveType(typeof(FormValueProviderFactory));
                options.ValueProviderFactories.Insert(0, new CustomNameFormValueProviderFactory());
            }

            if (options.Filters.All(x => x.GetType() != typeof(MiddlewareFilterAttribute)))
                options.Filters.Add(new MiddlewareFilterAttribute(typeof(LocalizationPipeline)));
        }

        services
            .AddMvc(options =>
            {
                AddMvcOptions(options);

                options.Conventions.Add(new LocalizedApplicationModelConvention());
            })
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
            })
            .AddRazorRuntimeCompilation();
        services.AddControllers(AddMvcOptions);
        var razorBuilder = services
            .AddRazorPages(options =>
            {
                _razorOptions?.Invoke(options);
                options.Conventions.Add(new LocalizedPageRouteModelConvention());
            });

        if (_webHostEnvironment.IsDevelopment() || _webHostEnvironment.IsStaging())
            razorBuilder.AddRazorRuntimeCompilation();

        var controllers = services.AddControllersWithViews(AddMvcOptions);

        // var moduleAssemblies = AppDomain.CurrentDomain.GetProjectAssemblies().Where(x => x.GetName().Name.StartsWith("Ecohos.Modules.Postings")).ToList();
        // foreach (var assembly in moduleAssemblies)
        //     controllers.AddApplicationPart(assembly);

        controllers = controllers.AddRazorRuntimeCompilation();

        // services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
        // {
        //     foreach (var assembly in moduleAssemblies)
        //         options.FileProviders.Add(new EmbeddedFileProvider(assembly));
        // });
        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = false;

            if (!options.ConstraintMap.ContainsKey(Constraints.LanguageKey))
                options.ConstraintMap.Add(Constraints.LanguageKey, typeof(LanguageRouteConstraint));
        });

        services.Configure<FormOptions>(options =>
        {
            const int megabytes = 10;
            options.MultipartBodyLengthLimit = megabytes * 1048576;
        });

        builder.Populate(services);
    }
}