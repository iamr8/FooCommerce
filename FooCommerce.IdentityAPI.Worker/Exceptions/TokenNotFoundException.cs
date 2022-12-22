namespace FooCommerce.IdentityAPI.Worker.Exceptions;

public class TokenNotFoundException : Exception
{
    public TokenNotFoundException() : base("Unable to find corresponding Token")
    {
    }
}