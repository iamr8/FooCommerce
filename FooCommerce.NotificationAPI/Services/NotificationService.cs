﻿using FooCommerce.Application.Notifications.Interfaces;
using FooCommerce.Application.Notifications.Services;
using FooCommerce.NotificationAPI.Models;

using Microsoft.Extensions.Configuration;

namespace FooCommerce.NotificationAPI.Services;

public class NotificationClientService : INotificationClientService
{
    private readonly IConfiguration _configuration;

    public NotificationClientService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEmailClientCredential GetAvailableMailboxCredentials()
    {
        var client = _configuration.GetSection("Emails:NoReply").Get<EmailClientCredential>();
        return client;
    }
}