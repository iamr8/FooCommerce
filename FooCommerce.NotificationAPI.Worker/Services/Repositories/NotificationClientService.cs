using FooCommerce.NotificationAPI.Interfaces;
using FooCommerce.NotificationAPI.Worker.Models;

namespace FooCommerce.NotificationAPI.Worker.Services.Repositories;

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