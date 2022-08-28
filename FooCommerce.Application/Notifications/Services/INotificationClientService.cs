using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.Application.Notifications.Services;

public interface INotificationClientService
{
    IEnumerable<IEmailClientCredential> GetAvailableMailboxCredentials();
}