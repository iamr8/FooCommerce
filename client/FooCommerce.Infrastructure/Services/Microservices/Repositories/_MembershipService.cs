using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

using FooCommerce.Domain.Helpers;
using FooCommerce.Infrastructure.Exceptions;
using FooCommerce.Infrastructure.Membership.Contracts;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FooCommerce.Infrastructure.Services.Microservices.Repositories;

public class _MembershipService : IMembershipClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly ILogger<_MembershipService> _logger;

    public _MembershipService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<_MembershipService> logger)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<Guid> RegisterAsync([NotNull] SignUpRequest model, CancellationToken cancellationToken = default)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        JsonObject json = null;
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
            var response = await _httpClient.PostAsync("api/membership/manager/index",
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, MediaTypeNames.Application.Json), cancellationToken);
            response.EnsureSuccessStatusCode();

            json = await JsonSerializer.DeserializeAsync<JsonObject>(await response.Content.ReadAsStreamAsync(), cancellationToken: cancellationToken);
            var hasCommId = json.TryGetGuid("commId", out var commId);
            if (!hasCommId)
                throw new NotRegisteredException("Unable to register user");

            return commId;
        }
        catch (NotRegisteredException e)
        {
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            if (json is not null)
            {
                var hasMessage = json.TryGetPropertyValue("message", out var message);
                if (hasMessage)
                    throw new NotRegisteredException(message.AsValue().ToString());
            }

            throw;
        }
    }
}