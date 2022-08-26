using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.NotificationAPI.Interfaces;

public interface IEmailClient : IDisposable, IAsyncDisposable, IEmailClientCredential
{
}