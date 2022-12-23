using System.Net.Mime;

using FooCommerce.Services.MembershipAPI.Contracts;
using FooCommerce.Services.MembershipAPI.Models;

using MassTransit;

using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.Services.MembershipAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class MembershipController : ControllerBase
{
    private readonly IRequestClient<CreateUser> _requestClient;

    private readonly ILogger<MembershipController> _logger;

    public MembershipController(ILogger<MembershipController> logger, IRequestClient<CreateUser> requestClient)
    {
        _logger = logger;
        _requestClient = requestClient;
    }

    /// <summary>
    /// Requests to create a new user.
    /// </summary>
    /// <param name="req"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A <see cref="Guid"/> value that represents User's Communication Id.</returns>
    /// <response code="201">A User has been created.</response>
    /// <response code="409">A User with the given email exists.</response>
    /// <response code="500">An internal server error occurred.</response>
    [HttpPost, Route("register")]
    [ProducesResponseType(typeof(RegisterResp), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(RegisterRespEmpty), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(RegisterRespFaulted), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RegisterRespFaulted), StatusCodes.Status500InternalServerError)]
    public async Task<IRegisterResp> Register(RegisterReq req, CancellationToken cancellationToken = default)
    {
        try
        {
            var resp = await _requestClient.GetResponse<UserCreated>(new
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email,
                Password = req.Password,
                //Country = req.Country
            }, cancellationToken);

            if (resp.Message.Success)
            {
                this.Response.StatusCode = StatusCodes.Status201Created;
                return new RegisterResp
                {
                    CommunicationId = resp.Message.CommId.Value
                };
            }
            else if (resp.Message.IsAlreadyExists)
            {
                this.Response.StatusCode = StatusCodes.Status409Conflict;
                return new RegisterRespEmpty();
            }
            else
            {
                this.Response.StatusCode = StatusCodes.Status400BadRequest;
                return new RegisterRespFaulted
                {
                    Message = resp.Message.Message
                };
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            this.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return new RegisterRespFaulted
            {
                Message = e.Message
            };
        }
    }
}