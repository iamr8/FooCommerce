using FooCommerce.Domain.ContextRequest;
using FooCommerce.Localization;
using FooCommerce.Services.NotificationAPI.Enums;
using FooCommerce.Services.NotificationAPI.Interfaces;

using MassTransit;

namespace FooCommerce.Services.NotificationAPI.Handlers;

public abstract class MessageHandler : IHandler
{
    protected readonly ILocalizer Localizer;
    protected readonly IBus Bus;

    protected MessageHandler(IBus bus, ILocalizer localizer)
    {
        Localizer = localizer;
        Bus = bus;
    }

    public abstract Task EnqueueAsync(ITemplate template, NotificationPurpose purpose, string receiverName,
        string receiverAddress,
        IDictionary<string, string> links, IDictionary<string, string> formatters, string websiteUrl,
        ContextRequestInfo requestInfo,
        CancellationToken cancellationToken = default);
}