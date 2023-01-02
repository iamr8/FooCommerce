namespace FooCommerce.Infrastructure.Services.Microservices.Repositories;

public class _BasketService : IBasketClient
{
    private readonly HttpClient _httpClient;

    public _BasketService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}