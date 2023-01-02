using System.Net.Mime;
using System.Text;
using System.Text.Json;

using FooCommerce.Infrastructure.Helpers;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FooCommerce.Infrastructure.Services.Microservices.Repositories;

public class _NotificationService : INotificationClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly ILogger<_NotificationService> _logger;

    public _NotificationService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<_NotificationService> logger)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task EnqueueAsync(string purpose, Guid receiverUserId)
    {
        if (purpose == null)
            throw new ArgumentNullException(nameof(purpose));

        // TODO: fetch receiver information from userId

        var requestContext = this._httpContextAccessor.HttpContext.GetRequestInfo();
        try
        {
            var payload = new
            {
                purpose = purpose, // "VERIFICATION_REQUEST_EMAIL",
                receiverName = "Arash",
                receiverCommunications = new Dictionary<string, string>
               {
                   {"EMAIL", "arash.shabbeh@gmail.com"},
                   {"SMS", "+905317251106"},
                   {"PUSH", "12312312312"}
               },
                baseUrl = "http://localhost:5000",
                requestInfo = requestContext,
            };
            var response = await _httpClient.PostAsync("api/notification/send",
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, MediaTypeNames.Application.Json));
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}