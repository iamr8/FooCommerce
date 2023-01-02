using System.Net.Mime;
using FooCommerce.Infrastructure.Membership.Contracts;
using FooCommerce.Infrastructure.Services.Microservices;
using FooCommerce.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class AuthController : ControllerBase
{
    private readonly IMembershipClient _membershipService;

    public AuthController(IMembershipClient membershipService)
    {
        _membershipService = membershipService;
    }

    [HttpPost, Route("register")]
    public async Task<RegisterResp> Register([FromBody] RegisterReq req, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ModelState.IsValid)
                return new RegisterResp();

            var commId = await _membershipService.RegisterAsync(new SignUpRequest
            {
                Email = req.Email,
                Password = req.Password,
                FirstName = req.FirstName,
                LastName = req.LastName
            }, cancellationToken);

            return new RegisterResp();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}