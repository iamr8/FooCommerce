using FooCommerce.NotificationAPI.Bridge.Interfaces;

namespace FooCommerce.NotificationAPI.Bridge.Services;

public interface INotificationClientService
{
    IEnumerable<IEmailClientCredential> GetAvailableMailboxCredentials();
}