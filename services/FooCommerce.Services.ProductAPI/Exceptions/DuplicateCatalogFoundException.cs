namespace FooCommerce.CatalogService.Exceptions;

public class DuplicateCatalogFoundException : Exception
{
    public string Name { get; }

    public DuplicateCatalogFoundException(string name) : base($"Catalog with name {name} already exists.")
    {
        Name = name;
    }
}