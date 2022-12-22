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

    [HttpPost, Route("send")]
    public async Task<SendResp> Send(SendReq req, CancellationToken cancellationToken = default)
    {
        try
        {
            var reqInfo = new ContextRequestInfo
            {
                Browser = req.RequestInfo.Browser,
                Device = req.RequestInfo.Device,
                IPAddress = IPAddress.Parse(req.RequestInfo.IPAddress),
                Platform = req.RequestInfo.Platform,
                UserAgent = req.RequestInfo.UserAgent,
                Country = new RegionInfo(req.RequestInfo.Country),
                Culture = CultureInfo.GetCultureInfo(req.RequestInfo.Culture),
                Engine = req.RequestInfo.Engine,
                TimezoneId = req.RequestInfo.TimezoneId,
            };
            
            await _service.EnqueueAsync(req.Purpose,
                req.ReceiverName,
                req.ReceiverCommunications,
                req.Links,
                req.Formatters,
                req.BaseUrl,
                reqInfo,
                cancellationToken);

            this.Response.StatusCode = StatusCodes.Status200OK;
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