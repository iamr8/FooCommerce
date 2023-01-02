namespace FooCommerce.Infrastructure.Services.Microservices.Repositories;

public class _VendorService : IVendorClient
{
    private readonly HttpClient _httpClient;

    public _VendorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}