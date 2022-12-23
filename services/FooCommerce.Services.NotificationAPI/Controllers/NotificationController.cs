using System.Globalization;
using System.Net;

using FooCommerce.Domain.ContextRequest;
using FooCommerce.Services.NotificationAPI.Models;
using FooCommerce.Services.NotificationAPI.Services;

using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.Services.NotificationAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly ICoordinator _service;

    private readonly ILogger<NotificationController> _logger;

    public NotificationController(ICoordinator service, ILogger<NotificationController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Send a notification to the user.
    /// </summary>
    /// <param name="req"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="202">Notification sent to queue.</response>
    /// <response code="400">Invalid request.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost, Route("send")]
    [ProducesResponseType(typeof(SendResp), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(SendResp), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SendResp), StatusCodes.Status500InternalServerError)]
    public async Task<SendResp> Send(SendReq req, CancellationToken cancellationToken = default)
    {
        try
        {
            var reqInfo = new ContextRequestInfo
            {
                Browser = req.Headers.Browser,
                Device = req.Headers.Device,
                IPAddress = IPAddress.Parse(req.Headers.IPAddress),
                Platform = req.Headers.Platform,
                UserAgent = req.Headers.UserAgent,
                Country = new RegionInfo(req.Headers.Country),
                Culture = CultureInfo.GetCultureInfo(req.Headers.Culture),
                Engine = req.Headers.Engine,
                TimezoneId = req.Headers.TimezoneId,
            };

            var communications = req.ReceiverCommunications.ToDictionary();
            if (!communications.Any())
            {
                this.Response.StatusCode = StatusCodes.Status400BadRequest;
                return new SendResp();
            }

            await _service.EnqueueAsync(req.Purpose,
                req.ReceiverName,
                communications,
                req.Links,
                req.Formatters,
                req.BaseUrl,
                reqInfo,
                cancellationToken);

            this.Response.StatusCode = StatusCodes.Status202Accepted;
            return new SendResp();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            this.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return new SendResp();
        }
    }
}