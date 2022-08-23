using System.Text;

using FooCommerce.Application.Interfaces.Notifications;

using MailKit.Net.Smtp;

using MimeKit;

namespace FooCommerce.NotificationAPI.Models;

public record EmailClient : IEmailClient
{
    private readonly SmtpClient _client;
    private readonly string _senderAddress;
    private readonly string _senderName;

    private EmailClient(SmtpClient client, string senderAddress, string senderName) : this()
    {
        _client = client;
        _senderAddress = senderAddress;
        _senderName = senderName;
    }

    private EmailClient()
    {
    }

    public static readonly EmailClient Empty = new();
    /// <summary>
    /// Creates an instance of <see cref="EmailClient"/>.
    /// </summary>
    /// <param name="senderAddress"></param>
    /// <param name="password"></param>
    /// <param name="server"></param>
    /// <param name="senderName"></param>
    /// <param name="smtpPort"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <returns></returns>
    public static async Task<EmailClient> GetInstanceAsync(string senderAddress, string password, string server, string senderName, int smtpPort, CancellationToken cancellationToken = default)
    {
        var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(server, smtpPort, true, cancellationToken);
            if (!client.IsConnected)
                throw new ArgumentException("Unable to connect to server.");

            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(senderAddress, password, cancellationToken);
            if (!client.IsAuthenticated)
                throw new ArgumentException("Unable to authenticate to server.");
        }
        catch (Exception)
        {
            await client.DisconnectAsync(true, cancellationToken);
            client.Dispose();
            throw;
        }

        return new EmailClient(client, senderAddress, senderName);
    }

    public async Task<bool> SendAsync(MimeMessage mailMessage, CancellationToken cancellationToken = default)
    {
        if (mailMessage == null) throw new ArgumentNullException(nameof(mailMessage));

        var senderName = _senderName;

        var subject = mailMessage.Subject;
        if (!string.IsNullOrEmpty(subject))
            senderName += $" - {subject}";

        mailMessage.From.Clear();
        var sender = new MailboxAddress(Encoding.UTF8, senderName, _senderAddress);
        mailMessage.From.Add(sender);

        try
        {
            await _client.SendAsync(mailMessage, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

        return true;
    }

    public void Dispose()
    {
        _client.Disconnect(true);
        _client.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _client.DisconnectAsync(true);
        _client.Dispose();
    }
}