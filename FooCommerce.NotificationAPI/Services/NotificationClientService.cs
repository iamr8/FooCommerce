using FooCommerce.Application.Notifications.Interfaces;
using FooCommerce.Application.Notifications.Services;
using FooCommerce.Core.Notifications.Interfaces;
using FooCommerce.NotificationAPI.Models;

using Microsoft.Extensions.Configuration;

namespace FooCommerce.NotificationAPI.Services;

public class NotificationClientService : INotificationClientService
{
    private readonly IConfiguration _configuration;

    public NotificationClientService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<IEmailClientCredential> GetAvailableMailboxCredentials()
    {
        return _configuration.GetSection("Emails").Get<List<EmailClientCredential>>();
    }
}