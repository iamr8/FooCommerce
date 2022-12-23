using System.Net.Mime;

using FooCommerce.Infrastructure.Membership.Contracts;
using FooCommerce.Infrastructure.Services;
using FooCommerce.Web.Models;

using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class AuthController : ControllerBase
{
    private readonly IMembershipService _membershipService;

    public AuthController(IMembershipService membershipService)
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