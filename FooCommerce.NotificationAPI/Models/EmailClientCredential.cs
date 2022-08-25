using FooCommerce.Application.Notifications.Interfaces;

namespace FooCommerce.NotificationAPI.Models;

public record EmailClientCredential(string SenderAlias, string SenderAddress, string Password, string Server, int SmtpPort, string Domain) : IEmailClientCredential;