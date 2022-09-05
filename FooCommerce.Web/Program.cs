using FooCommerce.AspNetCoreExtensions.Helpers;
using FooCommerce.Infrastructure.Bootstrapper;
using FooCommerce.Infrastructure.Bootstrapper.Mvc.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var ff = Environment.GetEnvironmentVariables();

builder.Services.ConfigureModules();

// In production, the React files will be served from this directory
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "wwwroot/build";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
    //app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

ContextRequestHelper.UseWebsiteUrl(ref app);

app.UseAuthorization();

var defaultCulture = app.Configuration.GetSection("SupportedLanguages").Get<string[]>()[0];
const string culturePattern = $"{LanguageConstraints.LanguageKey}:{LanguageConstraints.LanguageKey}";

app.MapControllerRoute(
    name: "default",
    pattern: "{" + culturePattern + "}/{controller}/{action=Index}/{id?}",
    defaults: new { culture = defaultCulture });

app.MapRazorPages();

app.MapFallbackToFile("index.html"); ;

app.Run();