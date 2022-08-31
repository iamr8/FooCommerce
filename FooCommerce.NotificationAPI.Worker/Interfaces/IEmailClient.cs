using FooCommerce.NotificationAPI.Interfaces;

namespace FooCommerce.NotificationAPI.Worker.Interfaces;

public interface IEmailClient : IDisposable, IAsyncDisposable, IEmailClientCredential
{
}