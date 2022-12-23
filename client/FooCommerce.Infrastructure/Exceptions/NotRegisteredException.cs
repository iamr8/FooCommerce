namespace FooCommerce.Infrastructure.Exceptions;

public class NotRegisteredException : Exception
{
    public NotRegisteredException(string s) : base(s)
    {
    }
}