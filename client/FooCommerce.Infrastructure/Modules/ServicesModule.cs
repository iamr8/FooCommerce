using FooCommerce.Common.Configurations;
using FooCommerce.Infrastructure.Services;
using FooCommerce.Infrastructure.Services.Microservices;
using FooCommerce.Infrastructure.Services.Microservices.Repositories;
using FooCommerce.Infrastructure.Services.Repositories;

using Microsoft.Extensions.DependencyInjection;

using Polly;
using Polly.CircuitBreaker;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Polly.Retry;

namespace FooCommerce.Infrastructure.Modules;

public class ServicesModule : Module
{
    public void Load(IServiceCollection services)
    {
        //services.AddScoped<ILocationService, LocationService>();

        services.AddHttpClient<INotificationClient, _NotificationService>(client =>
                client.BaseAddress = new Uri("https://localhost:5101"))
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddHttpClient<ITokenClient, _TokenService>(client =>
                client.BaseAddress = new Uri("https://localhost:5111"))
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddHttpClient<IMembershipClient, _MembershipService>(client =>
                client.BaseAddress = new Uri("https://localhost:5121"))
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddHttpClient<ICatalogClient, _CatalogService>(client =>
                client.BaseAddress = new Uri("https://localhost:5131"))
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddHttpClient<IBasketClient, _BasketService>(client =>
                client.BaseAddress = new Uri("https://localhost:5141"))
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddHttpClient<IVendorClient, _VendorService>(client =>
                client.BaseAddress = new Uri("https://localhost:5151"))
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddHttpClient<IPaymentClient, _PaymentService>(client =>
                client.BaseAddress = new Uri("https://localhost:5161"))
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddScoped<IListingService, ListingService>();
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