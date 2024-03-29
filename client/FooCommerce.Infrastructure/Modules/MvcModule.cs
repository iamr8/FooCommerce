﻿using FooCommerce.Common.Configurations;
using FooCommerce.Domain.Jsons;
using FooCommerce.Infrastructure.Mvc.Localization;
using FooCommerce.Infrastructure.Mvc.ModelBinders;
using FooCommerce.Infrastructure.Mvc.ModelBinding.CustomProviders;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Modules;

public class MvcModule : Module
{
    private readonly Action<RazorPagesOptions> _razorOptions;

    public MvcModule(Action<RazorPagesOptions> razorOptions = null)
    {
        _razorOptions = razorOptions;
    }

    public void Load(IServiceCollection services)
    {
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
                foreach (var jsonConverter in JsonDefaultSettings.Settings.Converters)
                {
                    var duplicateConverter = options.JsonSerializerOptions.Converters.Any(x => x.GetType() == jsonConverter.GetType());
                    if (!duplicateConverter)
                        options.JsonSerializerOptions.Converters.Add(jsonConverter);
                }

                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonDefaultSettings.Settings.DefaultIgnoreCondition;
                options.JsonSerializerOptions.UnknownTypeHandling = JsonDefaultSettings.Settings.UnknownTypeHandling;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonDefaultSettings.Settings.DictionaryKeyPolicy;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = JsonDefaultSettings.Settings.PropertyNameCaseInsensitive;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonDefaultSettings.Settings.PropertyNamingPolicy;
                options.JsonSerializerOptions.WriteIndented = true;
            });
        // .AddRazorRuntimeCompilation();
        services.AddControllers(AddMvcOptions);
        var razorBuilder = services
            .AddRazorPages(options =>
            {
                _razorOptions?.Invoke(options);
                options.Conventions.Add(new LocalizedPageRouteModelConvention());
            });

        // if (_webHostEnvironment.IsDevelopment() || _webHostEnvironment.IsStaging())
        // razorBuilder.AddRazorRuntimeCompilation();

        var controllers = services.AddControllersWithViews(AddMvcOptions);

        // var moduleAssemblies = AppDomain.CurrentDomain.GetProjectAssemblies().Where(x => x.GetName().Name.StartsWith("Ecohos.Modules.Postings")).ToList();
        // foreach (var assembly in moduleAssemblies)
        //     controllers.AddApplicationPart(assembly);

        //controllers = controllers.AddRazorRuntimeCompilation();

        // services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
        // {
        //     foreach (var assembly in moduleAssemblies)
        //         options.FileProviders.Add(new EmbeddedFileProvider(assembly));
        // });
        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = false;

            if (!options.ConstraintMap.ContainsKey(LanguageConstraints.LanguageKey))
                options.ConstraintMap.Add(LanguageConstraints.LanguageKey, typeof(LanguageRouteConstraint));
        });

        services.Configure<FormOptions>(options =>
        {
            const int megabytes = 10;
            options.MultipartBodyLengthLimit = megabytes * 1048576;
        });
    }
}