namespace FooCommerce.MembershipService.Contracts;

public record SignInRequest
{
    /// <summary>
    /// Can be either an email or a mobile number
    /// </summary>
    public string Username { get; init; }
    public string Password { get; init; }
    public bool Remember { get; init; }
    public string ReturnUrl { get; init; }
}