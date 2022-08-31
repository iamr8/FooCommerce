using FooCommerce.NotificationAPI.Interfaces;

namespace FooCommerce.NotificationAPI.Services;

public interface INotificationClientService
{
    IEnumerable<IEmailClientCredential> GetAvailableMailboxCredentials();
}