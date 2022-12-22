using FooCommerce.Services.TokenAPI.Contracts;
using FooCommerce.Services.TokenAPI.Enums;
using FooCommerce.Services.TokenAPI.Models;

using MassTransit;

using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.Services.TokenAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
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

    [HttpPost, Route("generate")]
    public async Task<GenerateTokenResp> GenerateToken(GenerateTokenReq req, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _requestCodeClient.GetResponse<TokenGenerationStatus>(new
            {
                IdentifierId = req.Identifier,
                Seconds = (int)req.Interval
            }, cancellationToken);

            this.Response.StatusCode = StatusCodes.Status200OK;
            return new GenerateTokenResp { Expiry = response.Message.ExpiresOn };
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            this.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return new GenerateTokenResp();
        }
    }

    [HttpPost, Route("validate")]
    public async Task<ValidateTokenResp> ValidateToken(ValidateTokenReq req, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _validateCodeClient.GetResponse<TokenValidationStatus>(new
            {
                IdentifierId = req.Identifier,
                Code = req.Code
            }, cancellationToken);

            this.Response.StatusCode = response.Message.Status switch
            {
                TokenStatus.Validated => StatusCodes.Status200OK,
                TokenStatus.TokenInvalid => StatusCodes.Status400BadRequest,
                TokenStatus.Expired => StatusCodes.Status404NotFound,
                TokenStatus.MaxRetryExceeded => StatusCodes.Status429TooManyRequests,
                TokenStatus.NotFound => StatusCodes.Status500InternalServerError,
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