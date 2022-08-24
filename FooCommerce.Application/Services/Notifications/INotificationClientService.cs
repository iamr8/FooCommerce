using FooCommerce.Application.Interfaces.Notifications;

namespace FooCommerce.Application.Services.Notifications;

public interface INotificationClientService
{
    IEmailClientCredential GetAvailableMailboxCredentials();
}