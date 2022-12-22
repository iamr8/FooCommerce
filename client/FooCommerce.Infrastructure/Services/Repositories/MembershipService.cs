using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

using FooCommerce.Infrastructure.Membership.Contracts;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FooCommerce.Infrastructure.Services.Repositories;

public class MembershipService : IMembershipService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly ILogger<MembershipService> _logger;

    public MembershipService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<MembershipService> logger)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<Guid> RegisterAsync(SignUpRequest model)
    {
        try
        {
            var payload = new
            {
                firstName = model.FirstName,
                lastName = model.LastName,
                email = model.Email,
                password = model.Password,
                //country = model.Country
            };
            var response = await _httpClient.PostAsync("api/Membership/register",
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, MediaTypeNames.Application.Json));
            response.EnsureSuccessStatusCode();

            var json = await JsonSerializer.DeserializeAsync<JsonNode>(await response.Content.ReadAsStreamAsync());
            if (json.AsObject().TryGetPropertyValue("commId", out var _commId))
            {
                if (Guid.TryParse(_commId.AsValue().ToString(), out var commId))
                {
                    if (commId != Guid.Empty)
                    {
                        return commId;
                    }
                }
            }

            throw new Exception("Unable to register user");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}