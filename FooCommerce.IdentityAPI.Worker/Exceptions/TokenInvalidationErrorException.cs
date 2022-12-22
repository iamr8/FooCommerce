namespace FooCommerce.IdentityAPI.Worker.Exceptions;

public class TokenInvalidationErrorException : Exception
{
    public TokenInvalidationErrorException() : base("Unable to invalidate token.")
    {
    }
}