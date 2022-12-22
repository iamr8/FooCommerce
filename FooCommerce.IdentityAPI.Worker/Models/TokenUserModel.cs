using FooCommerce.IdentityAPI.Worker.Dtos;

namespace FooCommerce.IdentityAPI.Worker.Models;

public record TokenUserModel : ITokenUserModel
{
    public Guid UserId { get; init; }
    public string Name { get; init; }
    public UserCommunicationModel Communication { get; init; }
}