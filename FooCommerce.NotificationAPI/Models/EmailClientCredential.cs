using FooCommerce.Application.Notifications.Interfaces;
using FooCommerce.Core.Notifications.Interfaces;

namespace FooCommerce.NotificationAPI.Models;

public record EmailClientCredential : IEmailClientCredential
{
    public string SenderAlias { get; init; }
    public string SenderAddress { get; init; }
    public string Password { get; init; }
    public string Server { get; init; }
    public int SmtpPort { get; init; }
}