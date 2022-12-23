using System.Net.Mime;
using FooCommerce.MembershipService.Contracts;
using FooCommerce.MembershipService.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.MembershipService.Controllers;

[Route("api/membership/manager")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class MembershipManagerController : ControllerBase
{
    private readonly IRequestClient<CreateUser> _requestClient;

    private readonly ILogger<MembershipManagerController> _logger;

    public MembershipManagerController(ILogger<MembershipManagerController> logger, IRequestClient<CreateUser> requestClient)
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
    /// <response code="400">An error occurred during the process.</response>
    /// <response code="409">A User with the given email exists.</response>
    /// <response code="500">An internal server error occurred.</response>
    [HttpPost, Route("index")]
    [ProducesResponseType(typeof(CreateResp), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(CreateReq req, CancellationToken cancellationToken = default)
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
                return Created("/api/membership/index", new CreateResp
                {
                    CommunicationId = resp.Message.CommId.Value
                });
            }
            else if (resp.Message.IsAlreadyExists)
            {
                return Conflict();
            }
            else
            {
                return BadRequest(resp.Message.Message);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}