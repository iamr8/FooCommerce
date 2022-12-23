namespace FooCommerce.Services.ProductAPI.Exceptions;

public class ParentCategoryNotFoundException : Exception
{
    public ParentCategoryNotFoundException(string name, int parentId)
    {
    }
}