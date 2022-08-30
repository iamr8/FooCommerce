using FooCommerce.NotificationAPI.Bridge.Interfaces;
using FooCommerce.NotificationAPI.Bridge.Services;
using FooCommerce.NotificationAPI.Models;

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