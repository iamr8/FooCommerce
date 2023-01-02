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

app.UseStaticFiles();

app.UseRouting();

//app.UseAuthorization();

//app.MapControllers();

//app.MapRazorPages();

app.MapFallbackToFile("index.html");

await app.RunAsync();