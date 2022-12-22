namespace FooCommerce.IdentityAPI.Worker.Exceptions;

public class TokenAdditionErrorException : Exception
{
    public TokenAdditionErrorException() : base("Unable to add the token model to the repository.")
    {
    }
}