using System.Text;

using FooCommerce.Application.Notifications.Interfaces;

using MailKit.Net.Smtp;

using MimeKit;

namespace FooCommerce.NotificationAPI.Models;

public record EmailClient : IEmailClient
{
    private readonly SmtpClient _client;
    public string SenderAddress { get; }
    public string Password { get; }
    public string Server { get; }
    public int SmtpPort { get; }
    public string SenderAlias { get; }

    private EmailClient(SmtpClient client, string senderAddress, string senderAlias, string password, string server, int smtpPort) : this()
    {
        _client = client;
        SenderAddress = senderAddress;
        SenderAlias = senderAlias;
        Password = password;
        Server = server;
        SmtpPort = smtpPort;
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
    /// <param name="senderAlias"></param>
    /// <param name="smtpPort"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <returns></returns>
    public static async Task<EmailClient> GetInstanceAsync(string senderAddress, string password, string server, string senderAlias, int smtpPort, CancellationToken cancellationToken = default)
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

        return new EmailClient(client, senderAddress, senderAlias, password, server, smtpPort);
    }

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