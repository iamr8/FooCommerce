using FooCommerce.IdentityAPI.Worker.Enums;

namespace FooCommerce.IdentityAPI.Worker.Models;

internal record TokenModel
{
    public string Code { get; init; }
    public Timer Timer { get; init; }
    public TimeSpan Timeout { get; init; }
    public TokenRequestPurpose Purpose { get; init; }
    public Guid UserCommunicationId { get; init; }
}