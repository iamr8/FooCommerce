using FooCommerce.Common.Configurations;
using FooCommerce.Infrastructure.Services;
using FooCommerce.Infrastructure.Services.Repositories;

using Microsoft.Extensions.DependencyInjection;

using Polly;
using Polly.CircuitBreaker;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Polly.Retry;

namespace FooCommerce.Infrastructure.Bootstrapper.Modules;

public class ServicesModule : Module
{
    public void Load(IServiceCollection services)
    {
        //services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<IAccountService, AccountService>();

        services.AddHttpClient<INotificationService, NotificationService>(client =>
                client.BaseAddress = new Uri("https://localhost:5101"))
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());
        
        services.AddHttpClient<ITokenService, TokenService>(client =>
                client.BaseAddress = new Uri("https://localhost:5111"))
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());
    }

    private AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 5));
    }

    private AsyncCircuitBreakerPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}