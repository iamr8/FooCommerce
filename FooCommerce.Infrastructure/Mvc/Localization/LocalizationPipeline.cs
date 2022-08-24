using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace FooCommerce.Infrastructure.Mvc.Localization;

public class LocalizationPipeline
{
    public void Configure(IApplicationBuilder app)
    {
        var configuration = (IConfiguration)app.ApplicationServices.GetService(typeof(IConfiguration));
        app.ConfigureLocalizationPipeline(configuration.GetSection("SupportedLanguages").Get<string[]>());
    }
}