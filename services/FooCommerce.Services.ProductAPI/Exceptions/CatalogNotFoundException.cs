namespace FooCommerce.CatalogService.Exceptions;

public class CatalogNotFoundException : Exception
{
    public string Slug { get; }
    public int? Id { get; }

    public CatalogNotFoundException(int id) : base($"Catalog with id {id} not found.")
    {
        Id = id;
    }

    public CatalogNotFoundException(string slug) : base($"Catalog with slug {slug} not found.")
    {
        Slug = slug;
    }
}