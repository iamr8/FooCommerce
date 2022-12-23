using System.Net.Mime;
using FooCommerce.Domain.Helpers;
using FooCommerce.Services.TokenAPI.Contracts;
using FooCommerce.Services.TokenAPI.Enums;
using FooCommerce.Services.TokenAPI.Models;

using MassTransit;

using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.Services.TokenAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class TokenController : ControllerBase
{
    private readonly IRequestClient<GenerateCode> _requestCodeClient;
    private readonly IRequestClient<ValidateCode> _validateCodeClient;

    private readonly ILogger<TokenController> _logger;

    public TokenController(IRequestClient<GenerateCode> requestCodeClient, IRequestClient<ValidateCode> validateCodeClient, ILogger<TokenController> logger)
    {
        _requestCodeClient = requestCodeClient;
        _validateCodeClient = validateCodeClient;
        _logger = logger;
    }

    /// <summary>
    /// Requests to generate a new code.
    /// </summary>
    /// <param name="req"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="201">A token has been created.</response>
    /// <response code="400">The given payload is not as expected.</response>
    /// <response code="500">An internal server error occurred.</response>
    [HttpPost, Route("generate")]
    [ProducesResponseType(typeof(GenerateTokenResp), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(GenerateTokenRespFaulted), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(GenerateTokenRespFaulted), StatusCodes.Status500InternalServerError)]
    public async Task<IGeneratedTokenResp> GenerateToken(GenerateTokenReq req, CancellationToken cancellationToken = default)
    {
        try
        {
            if (req.LifetimeInSeconds < 1000)
            {
                this.Response.StatusCode = StatusCodes.Status400BadRequest;
                return new GenerateTokenRespFaulted();
            }

            var response = await _requestCodeClient.GetResponse<TokenGenerated>(new
            {
                IdentifierId = req.Identifier,
                LifetimeInSeconds = (int)req.LifetimeInSeconds
            }, cancellationToken);

            this.Response.StatusCode = StatusCodes.Status201Created;
            return new GenerateTokenResp
            {
                TokenId = response.Message.CorrelationId,
                Expiry = response.Message.ExpiresOn.ToUnixTime(),
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            this.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return new GenerateTokenRespFaulted();
        }
    }

    /// <summary>
    /// Requests to validate a code.
    /// </summary>
    /// <param name="req"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="202">Token has been validated.</response>
    /// <response code="406">The given code is wrong.</response>
    /// <response code="408">Token is already expired.</response>
    /// <response code="429">Max retries on token check has been exceeded.</response>
    /// <response code="404">Token not found.</response>
    /// <response code="500">An internal server error occurred.</response>
    [HttpPost, Route("validate")]
    [ProducesResponseType(typeof(ValidateTokenResp), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ValidateTokenResp), StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(typeof(ValidateTokenResp), StatusCodes.Status408RequestTimeout)]
    [ProducesResponseType(typeof(ValidateTokenResp), StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ValidateTokenResp), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidateTokenResp), StatusCodes.Status500InternalServerError)]
    public async Task<ValidateTokenResp> ValidateToken(ValidateTokenReq req, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _validateCodeClient.GetResponse<TokenFulfilled>(new
            {
                CorrelationId = req.TokenId,
                Code = req.Code
            }, cancellationToken);

            this.Response.StatusCode = response.Message.Status switch
            {
                TokenStatus.Validated => StatusCodes.Status202Accepted,
                TokenStatus.CodeInvalid => StatusCodes.Status406NotAcceptable,
                TokenStatus.Expired => StatusCodes.Status408RequestTimeout,
                TokenStatus.MaxRetryExceeded => StatusCodes.Status429TooManyRequests,
                TokenStatus.NotFound => StatusCodes.Status404NotFound,
                _ => this.Response.StatusCode
            };

            return new ValidateTokenResp();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return new ValidateTokenResp();
        }
    }
}