using FooCommerce.Products.Interfaces;

namespace FooCommerce.Products.RealEstates.Models;

public record NewRealEstateAdRequest : IAdRequest
{
    public long CategoryId { get; set; }
}