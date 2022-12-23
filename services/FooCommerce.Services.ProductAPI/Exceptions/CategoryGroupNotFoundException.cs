namespace FooCommerce.Services.ProductAPI.Exceptions;

public class CategoryGroupNotFoundException : Exception
{
    public CategoryGroupNotFoundException(int groupId)
    {
    }
}