using FooCommerce.Domain.ContextRequest;
using FooCommerce.Domain.Enums;
using FooCommerce.Services.NotificationAPI.Enums;
using FooCommerce.Services.NotificationAPI.Exceptions;
using FooCommerce.Services.NotificationAPI.Models;

namespace FooCommerce.Services.NotificationAPI.Services;

public interface ICoordinator
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="purpose"></param>
    /// <param name="receiverName"></param>
    /// <param name="communications"></param>
    /// <param name="links"></param>
    /// <param name="formatters"></param>
    /// <param name="baseUrl"></param>
    /// <param name="requestInfo"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="TemplateModelNotFoundException"></exception>
    /// <exception cref="HandlerNotFoundException"></exception>
    Task EnqueueAsync(NotificationPurpose purpose,
        string receiverName,
        IDictionary<CommType, string> communications,
        IDictionary<string, string> links,
        IDictionary<string, string> formatters, string baseUrl,
        ContextRequestInfo requestInfo,
        CancellationToken cancellationToken = default);
}