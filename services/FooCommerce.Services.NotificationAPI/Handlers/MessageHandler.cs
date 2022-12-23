using FooCommerce.Domain.ContextRequest;
using FooCommerce.Localization;
using FooCommerce.NotificationService.Enums;
using FooCommerce.NotificationService.Interfaces;
using MassTransit;

namespace FooCommerce.NotificationService.Handlers;

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