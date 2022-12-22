using System.Text;

using FooCommerce.Services.NotificationAPI.Interfaces;

using MailKit.Net.Smtp;

using MimeKit;

namespace FooCommerce.Services.NotificationAPI.Models;

public record EmailClient : IEmailClient
{
    private SmtpClient _client;
    public string SenderAddress { get; set; }
    public string Password { get; set; }
    public string Server { get; set; }
    public int SmtpPort { get; set; }
    public string SenderAlias { get; set; }

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        _client = new SmtpClient();
        try
        {
            await _client.ConnectAsync(this.Server, this.SmtpPort, true, cancellationToken);
            if (!_client.IsConnected)
                throw new ArgumentException("Unable to connect to server.");

            _client.AuthenticationMechanisms.Remove("XOAUTH2");
            await _client.AuthenticateAsync(this.SenderAddress, this.Password, cancellationToken);
            if (!_client.IsAuthenticated)
                throw new ArgumentException("Unable to authenticate to server.");
        }
        catch (Exception)
        {
            await _client.DisconnectAsync(true, cancellationToken);
            _client.Dispose();
            throw;
        }
    }
    /// <summary>
    ///
    /// </summary>
    /// <param name="mailMessage"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<bool> SendAsync(MimeMessage mailMessage, CancellationToken cancellationToken = default)
    {
        if (mailMessage == null) throw new ArgumentNullException(nameof(mailMessage));

        var senderName = SenderAlias;

        var subject = mailMessage.Subject;
        if (!string.IsNullOrEmpty(subject))
            senderName += $" - {subject}";

        mailMessage.From.Clear();
        var sender = new MailboxAddress(Encoding.UTF8, senderName, SenderAddress);
        mailMessage.From.Add(sender);

        await _client.SendAsync(mailMessage, cancellationToken);
        return true;
    }

    public void Dispose()
    {
        if (_client is { IsConnected: true })
        {
            _client.Disconnect(true);
            _client.Dispose();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_client is { IsConnected: true })
        {
            await _client.DisconnectAsync(true);
            _client.Dispose();
        }
    }
}