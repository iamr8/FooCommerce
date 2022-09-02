using FooCommerce.NotificationAPI.Interfaces;

namespace FooCommerce.NotificationAPI.Worker.Services;

public interface INotificationClientService
{
    IEnumerable<IEmailClientCredential> GetAvailableMailboxCredentials();
}