using FooCommerce.Application.Interfaces.Notifications;

namespace FooCommerce.NotificationAPI.Models;

public record EmailClientCredential(string SenderAlias, string SenderAddress, string Password, string Server, int SmtpPort, string Domain) : IEmailClientCredential;