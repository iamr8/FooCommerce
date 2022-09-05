namespace FooCommerce.IdentityAPI.Contracts.Requests;

public record SignUpRequest
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public uint Country { get; init; }
}