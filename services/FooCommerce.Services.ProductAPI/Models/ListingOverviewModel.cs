namespace FooCommerce.CatalogService.Models;

public record ListingOverviewModel
{
    public uint Id { get; init; }
    public string Name { get; init; }
    public string Image { get; init; }
    public Dictionary<string,string> Specifications { get; init; }
    public int RatingsAverage { get; init; }
    public int RatingsCount { get; init; }
    public int Likes { get; init; }
    public decimal Price { get; set; }
}