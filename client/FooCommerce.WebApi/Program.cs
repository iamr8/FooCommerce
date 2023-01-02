using FooCommerce.Common.Helpers;
using FooCommerce.Infrastructure.Helpers;
using FooCommerce.Infrastructure.Modules;
using FooCommerce.Infrastructure.Mvc.Localization;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var connectionString = Environment.GetEnvironmentVariable("ConnectionString");

        builder.Services.AddService<AutoFluentValidationModule>();
        builder.Services.AddService(new MvcModule());
        builder.Services.AddService<ServicesModule>();
        builder.Services.AddService<ProtectionModule>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();

        app.UseRouting();

        HttpContextHelper.UseWebsiteUrl(ref app);

        app.UseAuthorization();

        var defaultCulture = app.Configuration.GetSection("SupportedLanguages").Get<string[]>()[0];
        const string culturePattern = $"{LanguageConstraints.LanguageKey}:{LanguageConstraints.LanguageKey}";

        app.MapControllers();
        app.MapControllerRoute(
            name: "default",
            pattern: "{" + culturePattern + "}/{controller}/{action=Index}/{id?}",
            defaults: new { culture = defaultCulture });

        app.MapRazorPages();

        await app.RunAsync();
    }
}