﻿namespace FooCommerce.NotificationAPI.Models;

public record EmailClientCredential(string Username, string Password, string Server, int SmtpPort, string SenderName, string Domain);