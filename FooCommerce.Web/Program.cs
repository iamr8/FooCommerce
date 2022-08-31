using Autofac;
using Autofac.Extensions.DependencyInjection;

using FooCommerce.Infrastructure.Bootstrapper;
using FooCommerce.Infrastructure.Bootstrapper.Mvc.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", true, true);
if (!string.IsNullOrEmpty(builder.Environment?.EnvironmentName))
{
    var path = $"appsettings.{builder.Environment.EnvironmentName}.json";
    builder.Configuration.AddJsonFile(path, true, true);
}
builder.Configuration.AddEnvironmentVariables();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
        containerBuilder.ConfigureAutofac(builder.Environment, connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

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