namespace FooCommerce.Services.ProductAPI.Exceptions;

public class DuplicateCategoryFoundException : Exception
{
    public DuplicateCategoryFoundException(string name)
    {
    }
}