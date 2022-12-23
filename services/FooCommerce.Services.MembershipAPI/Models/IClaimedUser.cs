using System.Security.Claims;
using FooCommerce.MembershipService.Enums;

namespace FooCommerce.MembershipService.Models;

public interface IClaimedUser
{
    /// <summary>
    /// Returns Username of the demanded user, according to <see cref="ClaimTypes.Name"/> claim.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Returns First name of the demanded user, according to <see cref="ClaimTypes.GivenName"/> claim.
    /// </summary>
    string GivenName { get; }

    /// <summary>
    /// Returns Last name of the demanded user, according to <see cref="ClaimTypes.Surname"/> claim.
    /// </summary>
    string Surname { get; }

    /// <summary>
    /// Returns Country of the demanded user, according to <see cref="ClaimTypes.Country"/> claim.
    /// </summary>
    uint Country { get; }

    /// <summary>
    /// Returns Role of the demanded user, according to <see cref="ClaimTypes.Role"/> claim.
    /// </summary>
    RoleType Role { get; }

    /// <summary>
    /// Returns Role Id of the demanded user, according to the Role stored in Database.
    /// </summary>
    Guid RoleId { get; }

    /// <summary>
    /// Returns Id of the demanded user, according to <see cref="ClaimTypes.NameIdentifier"/> claim.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Returns Hashed password of the demanded user, according to <see cref="ClaimTypes.Hash"/> claim.
    /// </summary>
    string Hash { get; }

    /// <summary>
    /// Returns Email address of the demanded user, according to <see cref="ClaimTypes.Email"/> claim.
    /// </summary>
    string Email { get; }

    /// <summary>
    /// Returns Mobile number of the demanded user, according to <see cref="ClaimTypes.MobilePhone"/> claim.
    /// </summary>
    string MobilePhone { get; }
}