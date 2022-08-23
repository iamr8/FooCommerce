using System.Globalization;

using FooCommerce.Application.Dtos.Membership;

namespace FooCommerce.Infrastructure.Membership.Models;

public class UserModel
{
    public Guid Id { get; init; }
    public string Hash { get; init; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public RegionInfo Country { get; set; }
    public RoleModel Role { get; set; }
}