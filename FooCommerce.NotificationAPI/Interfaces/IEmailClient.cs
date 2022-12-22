using MimeKit;

namespace FooCommerce.Services.NotificationAPI.Interfaces;

public interface IEmailClient : IDisposable, IAsyncDisposable
{
    string SenderAddress { get; }
    string Password { get; }
    string Server { get; }
    int SmtpPort { get; }
    string SenderAlias { get; }

    Task ConnectAsync(CancellationToken cancellationToken = default);
    Task<bool> SendAsync(MimeMessage mailMessage, CancellationToken cancellationToken = default);
}