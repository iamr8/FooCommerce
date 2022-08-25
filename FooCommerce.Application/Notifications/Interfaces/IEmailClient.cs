namespace FooCommerce.Application.Notifications.Interfaces;

public interface IEmailClient : IDisposable, IAsyncDisposable, IEmailClientCredential
{
}