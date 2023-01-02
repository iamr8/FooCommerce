namespace FooCommerce.Infrastructure.Services.Microservices.Repositories;

public class _PaymentService : IPaymentClient
{
    private readonly HttpClient _httpClient;

    public _PaymentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}