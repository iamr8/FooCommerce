using FooCommerce.Products.Interfaces;

namespace FooCommerce.Products.PurchasableItems.Models;

public record NewPurchasableItemRequest : IAdRequest
{
    public long CategoryId { get; set; }
    public string FullQualifiedName { get; set; }
    public string Name { get; set; }
    public Dictionary<long, object[]> Specifications { get; set; }
    public string[] Images { get; set; }
    public string Price { get; set; }
}