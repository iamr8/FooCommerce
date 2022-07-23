using FooCommerce.Products.Domain.Interfaces;

namespace FooCommerce.Products.RealEstates.Models;

public class NewRealEstateAdResponse : IAdRequestResponse
{
    public bool IsSuccess { get; set; }
}