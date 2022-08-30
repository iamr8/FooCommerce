using FooCommerce.NotificationAPI.Bridge.Interfaces;

namespace FooCommerce.NotificationAPI.Interfaces;

public interface IEmailClient : IDisposable, IAsyncDisposable, IEmailClientCredential
{
}