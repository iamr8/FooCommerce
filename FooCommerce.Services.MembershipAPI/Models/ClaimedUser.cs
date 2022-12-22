using System.Security.Claims;
using FooCommerce.Services.MembershipAPI.Enums;

namespace FooCommerce.Services.MembershipAPI.Models;

internal record ClaimedUser : IClaimedUser
{
    public ClaimedUser(ClaimsPrincipal principal)
    {
        Name = principal.FindFirst(ClaimTypes.Name).Value;
        GivenName = principal.FindFirst(ClaimTypes.GivenName).Value;
        Id = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier).Value);
        Email = principal.FindFirst(ClaimTypes.Email)?.Value;
        MobilePhone = principal.FindFirst(ClaimTypes.MobilePhone)?.Value;
        Hash = principal.FindFirst(ClaimTypes.Hash).Value;
        Role = Enum.Parse<RoleType>(principal.FindFirst(ClaimTypes.Role).Value);
        RoleId = Guid.Parse(principal.FindFirst("RoleId").Value);
        Surname = principal.FindFirst(ClaimTypes.Surname).Value;
        Country = uint.Parse(principal.FindFirst(ClaimTypes.Country).Value);
    }

    public string Name { get; init; }
    public string GivenName { get; init; }
    public string Surname { get; init; }
    public uint Country { get; init; }
    public RoleType Role { get; init; }
    public Guid RoleId { get; init; }
    public Guid Id { get; init; }
    public string Hash { get; init; }
    public string Email { get; init; }
    public string MobilePhone { get; init; }
}