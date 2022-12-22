using FooCommerce.Services.MembershipAPI.Contracts;
using FooCommerce.Services.MembershipAPI.Models;

using MassTransit;

using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.Services.MembershipAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MembershipController : ControllerBase
{
    private readonly IRequestClient<CreateUser> _requestClient;

    private readonly ILogger<MembershipController> _logger;

    public MembershipController(ILogger<MembershipController> logger, IRequestClient<CreateUser> requestClient)
    {
        _logger = logger;
        _requestClient = requestClient;
    }

    [HttpPost, Route("register")]
    public async Task<RegisterResp> Register(RegisterReq req, CancellationToken cancellationToken = default)
    {
        try
        {
            var resp = await _requestClient.GetResponse<UserCreationStatus>(new
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email,
                Password = req.Password,
                //Country = req.Country
            }, cancellationToken);

            if (!string.IsNullOrEmpty(resp.Message.ExceptionMessage))
                throw new Exception(resp.Message.ExceptionMessage);

            if (resp.Message.Fault != null)
                throw new Exception(resp.Message.Fault.Value.ToString());

            this.Response.StatusCode = StatusCodes.Status200OK;
            return new RegisterResp
            {
                CommunicationId = resp.Message.CommunicationId.Value
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            this.Response.StatusCode = StatusCodes.Status500InternalServerError;

            // Send status
            return new RegisterResp();
        }
    }
}