namespace FooCommerce.CatalogService.Exceptions;

public class CatalogNotFoundException : Exception
{
    public int? Id { get; }

    public CatalogNotFoundException(int id) : base($"Catalog with id {id} not found.")
    {
        Id = id;
    }
}