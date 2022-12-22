using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging;

namespace FooCommerce.Infrastructure.Services.Repositories;

public class TokenService : ITokenService
{
    private readonly HttpClient _httpClient;

    private readonly ILogger<TokenService> _logger;

    public TokenService(HttpClient httpClient, ILogger<TokenService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<DateTime?> GenerateAsync(Guid userCommunicationId, TimeSpan interval)
    {
        try
        {
            var payload = new
            {
                id = userCommunicationId,
                interval = (long)interval.TotalSeconds
            };
            var response = await _httpClient.PostAsync("api/Token/generate",
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var json = await JsonSerializer.DeserializeAsync<JsonNode>(await response.Content.ReadAsStreamAsync());

            if (json.AsObject().TryGetPropertyValue("expiry", out var expiry))
            {
                var expiryDateTime = DateTime.Parse(expiry.AsValue().ToString());
                return expiryDateTime;
            }

            return null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return null;
        }
    }

    public enum TokenStatus
    {
        Validated,
        Expired,
        TokenInvalid,
        MaxRetryExceeded
    }

    public async Task<TokenStatus> ValidateAsync(Guid userCommunicationId, string code)
    {
        try
        {
            var payload = new
            {
                id = userCommunicationId,
                code
            };
            var response = await _httpClient.PostAsync("api/Token/validate",
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        return TokenStatus.Validated;
                    }
                case HttpStatusCode.BadRequest:
                    {
                        return TokenStatus.TokenInvalid;
                    }
                case HttpStatusCode.NotFound:
                    {
                        return TokenStatus.Expired;
                    }
                case HttpStatusCode.TooManyRequests:
                    {
                        return TokenStatus.MaxRetryExceeded;
                    }
            }

            throw new Exception("Unexpected response from token service");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}