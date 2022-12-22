using FooCommerce.Domain.ContextRequest;
using FooCommerce.Services.NotificationAPI.Enums;
using FooCommerce.Services.NotificationAPI.Interfaces;
using FooCommerce.Services.NotificationAPI.Models;

namespace FooCommerce.Services.NotificationAPI.Handlers;

public interface IHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="template"></param>
    /// <param name="purpose"></param>
    /// <param name="receiverName"></param>
    /// <param name="receiverAddress"></param>
    /// <param name="links"></param>
    /// <param name="formatters"></param>
    /// <param name="websiteUrl"></param>
    /// <param name="requestInfo"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    Task EnqueueAsync(ITemplate template,
        NotificationPurpose purpose,
        string receiverName,
        string receiverAddress,
        IDictionary<string, string> links,
        IDictionary<string, string> formatters,
        string websiteUrl,
        ContextRequestInfo requestInfo,
        CancellationToken cancellationToken = default);
}