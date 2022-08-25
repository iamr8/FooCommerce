namespace FooCommerce.Application.Notifications.Interfaces;

public interface IEmailClientCredential
{
    string SenderAddress { get; }
    string Password { get; }
    string Server { get; }
    int SmtpPort { get; }
    string SenderAlias { get; }
}