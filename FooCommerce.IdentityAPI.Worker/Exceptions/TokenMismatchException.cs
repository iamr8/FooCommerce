namespace FooCommerce.IdentityAPI.Worker.Exceptions;

public class TokenMismatchException : Exception
{
    public TokenMismatchException() : base("The given token mismatched.")
    {
    }
}