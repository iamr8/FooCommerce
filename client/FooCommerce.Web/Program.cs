using FooCommerce.Common.Helpers;
using FooCommerce.Infrastructure.Helpers;
using FooCommerce.Infrastructure.Modules;
using FooCommerce.Infrastructure.Mvc.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost
    .UseContentRoot(Directory.GetCurrentDirectory())
    .UseKestrel()
    .UseSetting("detailedErrors", "true")
    .CaptureStartupErrors(true);

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var connectionString = Environment.GetEnvironmentVariable("ConnectionString");

builder.Services.AddService<AutoFluentValidationModule>();
builder.Services.AddService(new MvcModule());
builder.Services.AddService<ServicesModule>();
builder.Services.AddService<ProtectionModule>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// In production, the React files will be served from this directory
builder.Services.AddSpaStaticFiles(configuration => configuration.RootPath = "wwwroot/build");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
    //app.UseHttpsRedirection();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseRouting();

HttpContextHelper.UseWebsiteUrl(ref app);

app.UseAuthorization();

var defaultCulture = app.Configuration.GetSection("SupportedLanguages").Get<string[]>()[0];
const string culturePattern = $"{LanguageConstraints.LanguageKey}:{LanguageConstraints.LanguageKey}";

app.MapControllerRoute(
    name: "default",
    pattern: "{" + culturePattern + "}/{controller}/{action=Index}/{id?}",
    defaults: new { culture = defaultCulture });

app.MapRazorPages();

app.MapFallbackToFile("index.html");

await app.RunAsync();